using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NewsAPI.Data;
using NewsAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace NewsAPI
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
            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddControllers();

            //Utilizzo i servizi dedicati alla JWT 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
            .AddJwtBearer(options =>    
            {    
                options.TokenValidationParameters = new TokenValidationParameters    
                {    
                ValidateIssuer = true,    
                ValidateAudience = true,    
                ValidateLifetime = true,    
                ValidateIssuerSigningKey = true,    
                ValidIssuer = Configuration["Jwt:Issuer"],    
                ValidAudience = Configuration["Jwt:Issuer"],    
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))    
                };    
             });    
                 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsAPI", Version = "v1" });

                //Utilizzo securitySchema per permettere all'utente di inserire il token e
                //poter utilizzare l'api
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securitySchema, new[] { "Bearer" } 
                }
                });
    
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();
               app.UseSwagger();
               app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            //Utilizzo UseAuthorization per l'autorizzazione
            app.UseAuthentication();      
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
