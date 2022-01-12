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
using SmartAdmin.AppServices.CtaAcesso.Interfaces;
using SmartAdmin.Domain.Entities.CtAcesso;
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
        private readonly ICtaUsuarioService _userService;

        public UsuarioController([FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] ICtaUsuarioService userService, IMapper mapper, ApplicationTenant tenant)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tenant = tenant;
            _userService = userService;

        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResult<Token>>> Login([FromBody] AccessCredentials userCredentials,
            [FromServices] AccessManagerService accessManager)
        {

            try
            {
                var result = await accessManager.ValidateUserCredentials(userCredentials);

                if ( result == null || result == SignInResult.Failed)
                {
                    return BadRequest(new ApiResult<Token>()
                    {
                        Success = false,
                        Message = AppConst.INVALID_USER_OR_PWD,
                        Data = null
                    });
                }

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
        public async Task<ActionResult<ApiResult<Token>>> RefreshToken([FromBody] RefreshTokenData userToken,
            [FromServices] AccessManagerService accessManager)
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

            return new UnauthorizedObjectResult(new ApiResult<bool>
                {Success = false, Message = AppConst.INVALID_DATA_TOKEN});

        }

        [HttpPost("registra-usuario")]
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

        [HttpGet("lista-itens-menu/{idGrupoUsuario}/{idTenant}")]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResult<List<SmartNavigationMenu>>>> ListaItensMenu(int idGrupoUsuario,
            int idTenant)
        {
            try
            {
                var lstItensMenu = _userService.ListaItensMenu(x =>
                                                               x.IdGrupo == idGrupoUsuario && 
                                                               x.IdTenant == idTenant &&
                                                               x.Habilitado == "S");

                var itensMenu = PreparaItensMenu(lstItensMenu); //_mapper.Map<List<VwCtaItensMenuGrupo>, List<ItensMenuGrupo>>(lstItensMenu);


                return Ok(new ApiResult<List<SmartNavigationMenu>>()
                {
                    Success = true,
                    Data = itensMenu
                });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.ErrorToClient<bool>());
            }
        }

        private List<SmartNavigationMenu> PreparaItensMenu(List<VwCtaItensMenuGrupo> itensMenuGrupo)
        {

            var itensMenu = new List<SmartNavigationMenu>();

            var menuNivel1 = itensMenuGrupo.Where(x => x.Nivel == 1).ToList().OrderBy(m => m.Ordem);

            //Monta os Menu de Nivel 1
            foreach (var item1 in menuNivel1)
            {
                var menu1 = new SmartNavigationMenu
                {
                    Key = item1.IdMenu,
                    ParentKey = item1.IdMenuPai,
                    TituloMenu = item1.DescricaoMenu,
                    Area = "",
                    ActionName = "",
                    ControllerName = "",
                    Imagem = item1.Imagem,
                    Nivel = (int) item1.Nivel,
                    Tags = item1.Tags
                };

                var menuNivel2 = itensMenuGrupo.Where(x => x.Nivel == 2 && x.IdMenuPai == item1.IdMenu)
                    .ToList()
                    .OrderBy(m => m.Ordem);

                foreach (var item2 in menuNivel2)
                {
                    var menu2 = new SmartNavigationMenu
                    {
                        Key = item2.IdMenu,
                        ParentKey = item2.IdMenuPai,
                        Area = item2.Area,
                        TituloMenu = item2.DescricaoMenu,
                        ActionName = item2.ActionName,
                        ControllerName = item2.ControllerName,
                        Imagem = item2.Imagem,
                        Nivel = (int) item2.Nivel,
                        Tags = item2.Tags
                    };

                    var menuNivel3 = itensMenuGrupo.Where(x => x.Nivel == 3 && x.IdMenuPai == item2.IdMenu)
                        .ToList()
                        .OrderBy(m => m.Ordem);

                    foreach (var item3 in menuNivel3)
                    {
                        var menu3 = new SmartNavigationMenu
                        {
                            Key = item3.IdMenu,
                            ParentKey = item3.IdMenuPai,
                            Area = item3.Area,
                            TituloMenu = item3.DescricaoMenu,
                            ActionName = item3.ActionName,
                            ControllerName = item3.ControllerName,
                            Imagem = item3.Imagem,
                            Nivel = (int) item3.Nivel,
                            Tags = item3.Tags
                        };

                        if (menu2.SubMenus == null)
                        {
                            menu2.SubMenus = new List<SmartNavigationMenu>() {menu3};
                        }
                        else
                        {
                            menu2.SubMenus.Add(menu3);
                        }
                    }

                    if (menu1.SubMenus == null)
                    {
                        menu1.SubMenus = new List<SmartNavigationMenu>() {menu2};
                    }
                    else
                    {
                        menu1.SubMenus.Add(menu2);
                    }

                }

                itensMenu.Add(menu1);
            }


            return itensMenu;
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
