using Microsoft.Extensions.DependencyInjection;
using SmartAdmin.AppServices.CtaAcesso;
using SmartAdmin.AppServices.CtaAcesso.Interfaces;
using SmartAdmin.Domain.Entities.Tenants;
using SmartAdmin.Domain.Repositories.Interfaces.CtAcesso;
using SmartAdmin.Infra.Repositories.CtAcesso;

namespace SmartAdmin.AppServices.Extensions
{
    public static class AppServiceCollectionExtensions
    {
      
        public static void AddAppServices(this IServiceCollection services)
        {

            //services.AddScoped<ITenat, CtaUsuarioService>();

            services.AddSingleton<ICtaUsuarioService,CtaUsuarioService>();

        }
    }
}
