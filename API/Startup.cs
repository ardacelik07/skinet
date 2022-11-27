using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;


namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
    

        public Startup(IConfiguration config)
        {
             _config = config;
          
        
        }

       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IbasketRepository,basketRepository>();
             services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
             services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x =>
             x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
             services.AddDbContext<AppIdentityDbContext>(x => {
                x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
             });
             services.AddSingleton<IConnectionMultiplexer>( c => {
                var Configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(Configuration);

             });
             services.AddIdentityServices(_config);
             services.AddCors(opt =>
             {
                opt.AddPolicy("CorsPolicy", policy =>{
                     
                     policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
                
                

             });
             
             
            
              services.Configure<ApiBehaviorOptions>(options=>{
                options.InvalidModelStateResponseFactory =ActionContext =>
                {
                    var errors =ActionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse{
                        
                        Errors = errors
                    };
               
                return new BadRequestObjectResult(errorResponse);
                  };
              });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });

                var securitySchema = new OpenApiSecurityScheme{
                    Description ="JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme ="bearer",
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id ="Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer",securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement {{securitySchema,new [] {"Bearer"}}};
                c.AddSecurityRequirement(securityRequirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

             app.UseMiddleware<ExceptionMiddleware>();
               app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            if (env.IsDevelopment())
            {
               
               
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
