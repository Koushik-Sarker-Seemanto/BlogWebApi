using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModelsService.Configuration;
using ModelsService.Managers.PostManager;
using ModelsService.Managers.UserManager;
using PostHandlerService;
using WebService.Configuration;

namespace WebService
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
            services.AddControllers();

            services.AddCors();

            services.AddSingleton<IUserAuthenticator, UserAuthenticator>();
            services.AddSingleton<IPostHandler, PostHandler>();
            
            services.AddSingleton<IUserManager,UserManager>();
            services.AddSingleton<IPostManager, PostManager>();
            
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));
            
            services.AddSingleton<IDatabaseSettings>(sp => 
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            // Add Swagger
            services.AddSwaggerGen(e => 
            {
                e.SwaggerDoc("v1", new OpenApiInfo {Title="BlogWebApi", Version="v1"} );

                e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                e.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id="Bearer"}
                        },
                        new string[] {}
                    }
                });
            });

            // Add JWT
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(op => {
                    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(op=> {
                    op.SaveToken = true;
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
            
            app.UseHttpsRedirection();
            
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection("SwaggerOptions").Bind(swaggerOptions);
            app.UseSwagger(op => op.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI(op => {op.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);});

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}