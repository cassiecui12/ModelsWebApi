using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelsWebAPI.Data;
using ModelsWebAPI.Hubs;
using ModelsWebAPI.Middleware;

namespace ModelsWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ModelDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ModelDatabase")));

            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://models.zaidi.id.au", "http://localhost:4200").AllowCredentials();
                });
            });

            services.AddSignalR();

            services.AddTokenAuthentication(Configuration);  
            services.AddAuthorization();
            
            services.AddScoped<IModelDbContext, ModelDbContext>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("message");
            });
        }
    }
}