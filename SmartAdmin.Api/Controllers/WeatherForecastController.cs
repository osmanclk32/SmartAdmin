//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//using SmartAdmin.AppServices.CtaAcesso.Interfaces;

//namespace SmartAdmin.Api.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<WeatherForecastController> _logger;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            _logger = logger;
//        }

//        //[HttpGet]
//        //public IEnumerable<WeatherForecast> Get()
//        //{
//        //    var rng = new Random();
//        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//        //    {
//        //        Date = DateTime.Now.AddDays(index),
//        //        TemperatureC = rng.Next(-20, 55),
//        //        Summary = Summaries[rng.Next(Summaries.Length)]
//        //    })
//        //    .ToArray();
//        //}

//        [HttpGet]
//        public IEnumerable<Usuario> Get([FromServices] ICtaUsuarioService ctaUsuario)
//        {
//            List<Usuario> lista = new List<Usuario>();

//            //var lst = ctaUsuario.ListAll();

//            //foreach (var item in lst)
//            //{
//            //    lista.Add(new Usuario()
//            //    {
//            //        IdUsuario = item.IdUsuario,
//            //        Login = item.Login,
//            //        Administrador = item.Administrador,
//            //        Email = item.Email,
//            //        IdColaborador = item.IdColaborador,
//            //        IdTenant = item.IdTenant,
//            //        PrimeiroAcesso = item.PrimeiroAcesso,
//            //        Senha = item.Senha,
//            //        IdSituacaoCadastral = item.IdSituacaoCadastral
//            //    });

//            //}
//            return lista.ToArray();
//        }
//    }
//}
