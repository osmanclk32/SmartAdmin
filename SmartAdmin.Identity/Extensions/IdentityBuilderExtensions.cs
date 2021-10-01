using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SmartAdmin.Identity.ConnectionFactories;
using SmartAdmin.Identity.Interfaces;
using SmartAdmin.Identity.Models;
using SmartAdmin.Identity.Stores;

namespace Identity.Dapper.Extensions
{
   
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        /// Adiciona implementação Dapper de armazenamentos de identidade do ASP.NET Core.
        /// </summary>
        public static IdentityBuilder AddDapperIdentityStores(this IdentityBuilder builder,  string connectionString)
        {
            AddStores(builder.Services, connectionString);
            return builder;
        }

        private static void AddStores(IServiceCollection services, string connectionString)
        {

            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddScoped<IDatabaseConnectionFactory>(provider => new PostgresConnectionFactory(connectionString));
        }
    }
}
