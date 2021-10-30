using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Api.Dtos;
using SmartAdmin.Identity.Models;
using SmartAdmin.Infra;
using SmartAdmin.Api.Security;
using SmartAdmin.AppServices.CtaAcesso;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace SmartAdmin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationTenant _tenant;

        public UsuarioController([FromServices] UserManager<ApplicationUser> userManager,IMapper mapper,ApplicationTenant tenant)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tenant = tenant;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResult<Token>>> Login([FromBody]AccessCredentials userCredentials, [FromServices] AccessManagerService accessManager)
        {

            try
            {
                var result = await accessManager.ValidateUserCredentials(userCredentials);

                if (result.Succeeded)
                {
                    var ipUser = IpAddress();

                    var token = await accessManager.GenerateToken(userCredentials.Email, ipUser);
                    
                    return Ok(new ApiResult<Token>()
                    {
                        Success = true,
                        Data = token,
                        Message = AppConst.AUTH_SUCCESS
                    });
                }
                
                if (result == SignInResult.Failed)
                {
                    return BadRequest(new ApiResult<Token>()
                    {
                        Success = false,
                        Message = AppConst.INVALID_USER_OR_PWD,
                        Data = null
                    });
                }
                
                if (result.IsNotAllowed)
                {
                    return Unauthorized(new ApiResult<Token>()
                    {
                        Success = false,
                        Message = AppConst.USER_NOT_ALLOWED,
                        Data = null
                    });
                }

                if (result.IsLockedOut)
                {
                    return Unauthorized(new ApiResult<Token>()
                    {
                        Success = false,
                        Message = AppConst.USER_IS_LOCKED_OUT,
                        Data = null
                    });
                }

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.ErrorToClient<bool>());
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResult<Token>>> RefreshToken([FromBody]RefreshTokenData userToken, [FromServices] AccessManagerService accessManager)
        {

            var ipUser = IpAddress();

            var token = await accessManager.RefreshToken(userToken, ipUser);

            if (token != null)
            {
                return Ok(new ApiResult<Token>()
                {
                    Success = true,
                    Data = token,
                    Message = AppConst.AUTH_SUCCESS
                });
            }

            return new UnauthorizedObjectResult(new ApiResult<bool> {Success = false, Message = AppConst.INVALID_DATA_TOKEN});

        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserApp usuario)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(usuario);

                var result = await _userManager.CreateAsync(user, usuario.Senha);

                var userToReturn = _mapper.Map<UserApp>(user);

                if (result.Succeeded)
                {
                    return Created("GetUser", userToReturn);
                }

                return BadRequest(result.Errors);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
            }
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }

            return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}
