using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAutores.Controllers;
using WebApiAutores.Servicios;

namespace WebApiAutores
{
    public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get;  }

        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddSingleton<IServicio, ServicioA>();

            //DISTINTOS SERVICIOS:
           // AddTransient == Agregar un transitorio, se nos va a dar una nueva instancia
           // AddScope == siempre distinta instancia al distintos usuarios
           // AdSingleton == siempre la misma instancia con distintos usuarios




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( Endpoint =>
                {
                Endpoint.MapControllers();
            });

        }
    }
}
