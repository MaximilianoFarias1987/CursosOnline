using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.DapperConexion;
using Persistencia.DapperConexion.Paginacion;
using Seguridad.Token;
using System.Text;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CursosContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<ConexionDapper>(Configuration.GetSection("DefaultConnection"));

            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<InsertarCurso>());

            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<CursosContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            services.AddScoped<IJwtGenerador, JwtGenerador>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
            services.AddAutoMapper(typeof(Consulta.Manejador));

            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IPaginacion, PaginacionRepositorio>();
            //services.AddAutoMapper(typeof(ConsultaId.Manejador));

            //services.AddControllers().AddNewtonsoftJson(options =>
            //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Servicios para mantenimiento de cursos",
                    Version = "v1"
                });
                c.CustomSchemaIds(c => c.FullName);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Cursos Online v1");
            });
        }
    }
}
