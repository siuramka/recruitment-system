using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.DataAccess.Seeders;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddIdentity<SiteUser, IdentityRole>()
            .AddEntityFrameworkStores<RecruitmentDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddDbContext<RecruitmentDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("RecruitmentSystem.DataAccess"));
        });


        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
                options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 };

                // options.TokenValidationParameters.ClockSkew = TimeSpan.Zero; needed if exp time < 5 minutes
            });
        
        builder.Services.AddScoped<StepSeeder>();

        var app = builder.Build();

        app.UseCors(options =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyMethod();
            options.AllowAnyHeader();
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        
        using var scope = app.Services.CreateScope();
        var dbSeeder = scope.ServiceProvider.GetRequiredService<StepSeeder>();
        dbSeeder.SeedSteps();
        
        app.Run();
    }
}