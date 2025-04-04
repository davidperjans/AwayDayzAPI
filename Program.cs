
using System.Security.Claims;
using System.Text;
using AwayDayzAPI.Database;
using AwayDayzAPI.Models;
using AwayDayzAPI.Seeder;
using AwayDayzAPI.Services.Admin;
using AwayDayzAPI.Services.Auth;
using AwayDayzAPI.Services.FootballApiService;
using AwayDayzAPI.Services.FriendRequestFolder;
using AwayDayzAPI.Services.FriendShip;
using AwayDayzAPI.Services.PostService;
using AwayDayzAPI.Services.Token;
using AwayDayzAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AwayDayzAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            // Configure Swagger with JWT authentication support
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AwayDayz API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                options.AddSecurityDefinition("Bearer", securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[] {} }
                });
            });

            // Configure database context
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Configure Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT authentication
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Add Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            // Configure Dependency Injection
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
            builder.Services.AddScoped<IFriendshipService, FriendshipService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<DatabaseSeeder>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IFootballApiService, FootballApiService>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

            // Configure CORS policy (Adjust allowed origins later) (TODO)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });


            var app = builder.Build();

            // Seed the database with initial data
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            seeder.SeedUsersAsync().Wait();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
