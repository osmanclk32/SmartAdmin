using Microsoft.Extensions.DependencyInjection;
using SmartAdmin.Domain.Repositories.Interfaces;
using SmartAdmin.Infra.Repositories;
using SmartAdmin.Infra.Repositories.Tenants;

namespace SmartAdmin.Infra.Extensions
{
    public static class AddInfraServiceCollections
    {
        /// <summary>
        /// Adiciona os repositorios de SmartAdmin.Infra para injeção de dependencia
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ISiltTenantsRepository,SiltTenantsRepository>();

            // services.AddSingleton<ITenat>();
        }
    }
}
