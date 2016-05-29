using LearningAspNetCore.Services;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearningAspNetCore
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(provider => Configuration);
            services.AddSingleton<IGreeter, Greeter>();
        }

        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment environment, IGreeter greeter)
        {
            app.UseIISPlatformHandler();
            //app.UseWelcomePage();
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDefaultFiles();

            //// if the request file found, it will stop go to next middleware
            //app.UseStaticFiles();

            app.UseMvc(b =>
            {
                b.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseRuntimeInfoPage("/info");

            app.Run(async context =>
            {
                var greeting = greeter.GetGreeting();

                await context.Response.WriteAsync(greeting);
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}