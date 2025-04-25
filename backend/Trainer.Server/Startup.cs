namespace Trainer.Server
{
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Trainer.Server.Data;
    using Trainer.Server.Helpers;
    using Trainer.Server.Interfaces;
    using Trainer.Server.Services.AuthService;
    using Trainer.Server.Services.TokenService;
    using Trainer.Server.Services.UserService;
    using Trainer.Server.Services.WorkoutService;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Dependency Injection
            services.AddScoped<MongoDbService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //Jwt Authentication Setup
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]!)),
                    };
                });

            services.AddAuthorization();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if(!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trainer API V1");
                c.RoutePrefix = string.Empty;
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id}"
            );
        }
    }
}
