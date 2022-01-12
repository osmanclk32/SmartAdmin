using Microsoft.Extensions.DependencyInjection;
using SmartAdmin.AppServices.CtaAcesso;
using SmartAdmin.AppServices.CtaAcesso.Interfaces;
using SmartAdmin.Domain.Entities.Tenants;
using SmartAdmin.Domain.Repositories.Interfaces;
using SmartAdmin.Domain.Repositories.Interfaces.CtAcesso;
using SmartAdmin.Identity.Models;
using SmartAdmin.Infra.Repositories.CtAcesso;

namespace SmartAdmin.AppServices.Extensions
{
    public static class AppServiceCollectionExtensions
    {
      
        public static void AddAppServices(this IServiceCollection services)
        {
            //Cria instancias de Repositorios cada vez que for acessado
            services.AddTransient<ISiltTokensApi, SiltTokensApiRepository>();
            services.AddTransient<ICtaUsuario, CtaUsuarioRepository>();
            services.AddTransient<IVwCtaItensMenuGrupo, VwCtaItensMenuGrupoRepository>();

            //Cria innstacias de Serviços apenas uma vez (primeira solicitação)
            services.AddScoped<ISiltTokensApiService, SiltTokensApiService>();

            services.AddSingleton<ICtaUsuarioService, CtaUsuarioService>();
            
        }
    }
}
