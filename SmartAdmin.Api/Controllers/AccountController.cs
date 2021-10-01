//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.Encodings.Web;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.WebUtilities;

//using SmartAdmin.Identity.Models;
//using Microsoft.Extensions.Logging;
//using AutoMapper;

//namespace SmartAdmin.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AccountController : ControllerBase
//    {
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IMapper _mapper;

//        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _mapper = mapper;
//        }

//        [AllowAnonymous]
//        [HttpPost("cria-usuario")]
//        public async Task<IActionResult> CriaUsuario([FromBody] Usuario usuario)
//        {
//            var user = _mapper.Map<ApplicationUser>(usuario);

//            //var user = new ApplicationUser
//            //{
//            //    IdTenant = usuario.IdTenant, Email = usuario.Email,NomeUsuario = usuario.NomeUsuario,IdColaborador = usuario.IdColaborador,Bloqueado = usuario.Bloqueado

//            //};

//            var result = await _userManager.CreateAsync(user, usuario.Senha);

//            if (result.Succeeded)
//            {
//                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

//              //  code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
////                var callbackUrl = Url.Page(
////                        "/Account/ConfirmEmail",
////                        pageHandler: null,
////                        values: new { area = "Identity", userId = user.Id, code },
////                        protocol: Request.Scheme);

////                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
////                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

////                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
////                    {
////                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
////}
////                    else
////                    {
////                        await _signInManager.SignInAsync(user, isPersistent: false);
////                        return LocalRedirect(returnUrl);
////                    }
            
//            }

//            return Ok(result);
//        }
//    }
//}
