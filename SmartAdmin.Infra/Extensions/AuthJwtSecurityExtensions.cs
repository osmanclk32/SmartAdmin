using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using SmartAdmin.Infra.Configuration;

namespace SmartAdmin.Infra.Extensions
{
    public static class AuthJwtSecurityExtensions
    {
        /// <summary>
        /// Adiciona configuração de autenticação e autorização via token JWT
        /// </summary>
        /// <param name="services"></param>
        /// <param name="signingConfigurations"></param>
        /// <param name="tokenConfigurations"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthJwtSecurity(this IServiceCollection services, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                
                paramsValidation.IssuerSigningKey = signingConfigurations.SigningCredentials.Key;
                paramsValidation.ValidAudience =  tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;

                //Adicionar um Token-Expired no cabeçalho de resposta quando uma solicitação chegar com um token expirado,
                //com isso o cliente pode usar essas informações para decidir usar o token de atualização (RefreshToken)
                bearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("TOKEN-EXPIRADO", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // Ativa o uso do token como forma de autorizar o acesso aos recursos da API
            services.AddAuthorization(auth => auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build()
                    ));

            return services;
        }
    }
}
