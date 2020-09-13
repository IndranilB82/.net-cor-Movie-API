using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movie.Demo.Repository.IInterface;
using Movie.Demo.Repository.Implimentation;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Movie.Demo.API.Helpers;
using Movie.Demo.API.Loggers;
using Movie.Demo.Utility.Enumeration;

namespace Movie.Demo.API
{
    public class Startup
    {
        private static Serilog.ILogger _Logger = Log.ForContext<Startup>();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IMovieRepository, MovieRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
            services.AddMvc();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "Movie Demo API", Version = "v1" });
            });
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                ConfigLogParams.LogLevel.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(ConfigLogParams.LogLevel)
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Debug)
                    .WriteTo.Logger(Log.Logger)
                    .WriteTo.Seq(Configuration.GetSection(URL.SeqUrl.ToString()).Value)
                    .CreateLogger();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(
                    options => options.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                   .AllowAnyHeader());

            app.ConfigureExceptionHandler(_Logger);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Demo API");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
