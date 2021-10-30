using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartAdmin.Api.Dtos;
using SmartAdmin.Infra.Configuration;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using SmartAdmin.Identity.Models;
using System.Reflection;
using SmartAdmin.AppServices.CtaAcesso;
using SmartAdmin.AppServices.CtaAcesso.Interfaces;

namespace SmartAdmin.Api.Security
{
    public class AccessManagerService
    {
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;
        private IDistributedCache _cache;
        private string _role;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISiltTokensApiService _tokensApiService;

        /// <summary>
        /// Se TRUE indica que é o primeiro acesso do usuario e que o mesmo deve realizar a troca da 
        /// senha para obter o token nos proximos acessos
        /// </summary>
        public bool FirstAccess { get; private set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public AccessManagerService([FromServices] UserManager<ApplicationUser> userManager,
                                    [FromServices] SignInManager<ApplicationUser> signInManager,
                                    [FromServices] SigningConfigurations signingConfigurations,
                                    [FromServices] TokenConfigurations tokenConfigurations,
                                    [FromServices] IDistributedCache cache,
                                    [FromServices] ISiltTokensApiService tokensApiService)
        {
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
            _cache = cache;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokensApiService = tokensApiService;
        }

        public async Task<SignInResult> ValidateUserCredentials(AccessCredentials userCredentials)
        {
            SignInResult logInResult = default;

            try
            {
                if ( !String.IsNullOrWhiteSpace(userCredentials.Email)  && !String.IsNullOrWhiteSpace(userCredentials.AccessKey))
                {
                    if (userCredentials.GrantType == AppConst.GRANT_TYPE_PASSWORD)
                    {
                        // Localiza o usuário com base no email informado
                        var userIdentity =  await _userManager.FindByEmailAsync(userCredentials.Email); // .FindByNameAsync(userCredentials.UserName);

                        if (userIdentity != null)
                        {
                            // Efetua o login com base no Id do usuário e sua senha
                            logInResult = await _signInManager.PasswordSignInAsync(userIdentity.NomeUsuario, userCredentials.AccessKey, userCredentials.RememberMe,  true);
                        }
                    }
                    else if (userCredentials.GrantType == AppConst.GRANT_TYPE_REFRESH)
                    {
                        if (!String.IsNullOrWhiteSpace(userCredentials.RefreshToken))
                        {
                            RefreshTokenData refreshTokenBase = null;

                            var tokenArmazenado = await _cache.GetStringAsync(userCredentials.RefreshToken);

                            _role = await _cache.GetStringAsync(userCredentials.UserName);

                            if (!String.IsNullOrWhiteSpace(tokenArmazenado))
                            {
                                refreshTokenBase = JsonConvert.DeserializeObject<RefreshTokenData>(tokenArmazenado);
                            }

                            var credenciaisValidas = (refreshTokenBase != null && userCredentials.UserName == refreshTokenBase.UserName && userCredentials.RefreshToken == refreshTokenBase.RefreshToken);

                            // Elimina o token de refresh já que um novo será gerado
                            if (credenciaisValidas)
                            {
                               await _cache.RemoveAsync(userCredentials.RefreshToken);

                                logInResult = SignInResult.Success;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return logInResult;
        }
        
        public async Task<Token> GenerateToken(string emailUser,string ipAddress)
        {
            IList<Claim> claims = new List<Claim>();

            try
            {
                var userIdentity = await _userManager.FindByEmailAsync(emailUser);

                Claim claim1 = new Claim(ClaimTypes.NameIdentifier, userIdentity.NomeUsuario);

                claims.Add(claim1);
                
                var roles = await _userManager.GetRolesAsync(userIdentity);

                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
                
                //Criando uma Identidade e associando-a ao ambiente.
                var identity = new ClaimsIdentity(claims, "SmartAdmin.Api");
                var principal = new ClaimsPrincipal(identity);

                Thread.CurrentPrincipal = principal;

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = default;
                
                dataExpiracao = dataCriacao + TimeSpan.FromHours(_tokenConfigurations.Hours) + TimeSpan.FromMinutes(_tokenConfigurations.Minutes) + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

                // Calcula o tempo máximo de validade do refresh token
                // (o mesmo será invalidado automaticamente)
                TimeSpan finalExpiration = TimeSpan.FromSeconds(_tokenConfigurations.FinalExpiration);

                var handler = new JwtSecurityTokenHandler();

                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _tokenConfigurations.Issuer,
                    Audience = _tokenConfigurations.Audience,
                    SigningCredentials = _signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });

                var token = handler.WriteToken(securityToken);

                var result = new Token
                {
                    Authenticated = true,
                    CreationDate = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    AccessToken = token,
                    RefreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty),
                    FirstAccess = this.FirstAccess,
                    UserName = userIdentity.NomeUsuario,
                    Email = userIdentity.Email,
                    Message = "OK"
                };

                // Armazena o refresh token e a role do usurio em cache através do Redis 
                var refreshTokenData = new RefreshTokenData();

                refreshTokenData.RefreshToken = result.RefreshToken;

                refreshTokenData.UserName = userIdentity.NomeUsuario;

                var opcoesCache = new DistributedCacheEntryOptions();

                opcoesCache.SetAbsoluteExpiration(finalExpiration);

                _cache.SetString(result.RefreshToken, JsonConvert.SerializeObject(refreshTokenData), opcoesCache);

                _tokensApiService.StoreToken(userIdentity.IdUsuario, ipAddress, result.RefreshToken,result.CreationDate,result.Expiration);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Token> RefreshToken(RefreshTokenData userToken, string ipAddress)
        {
            try
            {
                var userIdentity = await _userManager.FindByNameAsync(userToken.UserName);

                if (userIdentity != null)
                {
                    var activeToken = _tokensApiService.Find(t => t.IdUsuario == userIdentity.IdUsuario && t.RefreshToken == userToken.RefreshToken && t.Ativo);

                    if (activeToken != null)
                    {
                        return await GenerateToken(userIdentity.Email, ipAddress);
                    }
                }
            }
            catch (Exception e)
            {
               
                throw;
            }

            return null;
        }
    }
}
