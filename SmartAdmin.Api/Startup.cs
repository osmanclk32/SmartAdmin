using System.Data.Common;
using System.Text.Json;

using DapperExt;
using Identity.Dapper.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using SmartAdmin.Api.Resolvers;
using SmartAdmin.Api.Security;
using SmartAdmin.AppServices.Extensions;
using SmartAdmin.Identity.Models;
using SmartAdmin.Infra.Configuration;
using SmartAdmin.Infra.Extensions;
using SmartAdmin.Infra.Security;

namespace SmartAdmin.Api
{
    public class Startup
    {
        private readonly string _corsPolicyName = "SmartAdminApiPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["SmartBusiness:ConnectionString"];

            DbProviderFactories.RegisterFactory("Npgsql", "Npgsql.NpgsqlFactory, Npgsql");

            var factory = DbProviderFactories.GetFactory("Npgsql");

            var aEsCrypt = new AESCrypt();

            var decryptedConnectionString = aEsCrypt.Decrypt(connectionString);

            services.AddTransient<IDapperDbContext>(p => new DapperDbContext(decryptedConnectionString, factory));

            services.AddDapperDbContext(decryptedConnectionString, factory);
            services.AddRepositories(); //Configurações dos repositorios para injeção de dependência

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMultitenancy<ApplicationTenant, TenantResolver>();

            services.AddAuthentication("Identity.Application")
                .AddCookie("Identity.Application", options =>
                {
                    options.Cookie.Domain = "smartadmin.com.br";
                    options.Cookie.Name = "SmartAdmin.Cookies";
                });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            //Configura as regras para senha do usuário
            var builder = services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false; //alterar para true quando estiver em produção
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4; //alterar esse valor quando estiver em produção

                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Lockout.AllowedForNewUsers = false;
                options.Stores.ProtectPersonalData = false;
                options.User.RequireUniqueEmail = false;
            });

            //Configura os gerenciadores de usuário e roles(grupos)
            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);

            builder.AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddRoleValidator<RoleValidator<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddDapperIdentityStores(decryptedConnectionString)
                .AddDefaultTokenProviders();


            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.IgnoreNullValues = true;
            });

            var tokenConfiguration = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(Configuration.GetSection("TokenConfiguration"))
                .Configure(tokenConfiguration);
            
            services.AddSingleton(tokenConfiguration);

            var signingConfigurations = new SigningConfigurations();

            services.AddSingleton(signingConfigurations);

            services.AddAuthJwtSecurity(signingConfigurations, tokenConfiguration);

            //Injeção de Dependencia para os serviços contidos em SmartAdmin.AppServices
            services.AddAppServices();

            //Cria instância apenas uma vez (primeira solicitação) para validar os dados de acesso.
            services.AddScoped<AccessManagerService>();
            
            services.AddAutoMapper(typeof(Startup));

            services.AddDistributedMemoryCache();

            services.AddCors(o => o.AddPolicy(_corsPolicyName, b =>
            {
                      b.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.Configure<IISOptions>(opt =>
            {
                opt.ForwardClientCertificate = false;
            });

           // services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "SmartAdmin.Api", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Autenticação baseada em Json Web Token (JWT)",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartAdmin.Api v1"));
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var execptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var execption = execptionHandlerPathFeature.Error;
                var result = JsonConvert.SerializeObject(new {error = execption.Message});

                context.Response.ContentType = "appliction/json";

                await context.Response.WriteAsync(result);

            }));

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseCors(_corsPolicyName);

            app.UseMultitenancy<ApplicationTenant>();

            app.UseAuthentication();
            
            app.UseStaticFiles();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
        }
    }
}