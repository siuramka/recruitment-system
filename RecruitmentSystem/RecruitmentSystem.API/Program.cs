using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.DataAccess.Seeders;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        builder.Services.AddAutoMapper(typeof(Program)); 
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "RecruitmentAPI", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
            

        });
        
        builder.Services.AddIdentity<SiteUser, IdentityRole>()
            .AddEntityFrameworkStores<RecruitmentDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddDbContext<RecruitmentDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                x => x.MigrationsAssembly("RecruitmentSystem.DataAccess"));
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
        
        builder.Services.AddScoped<DataSeeder>();
        builder.Services.AddScoped<JwtService>();
        builder.Services.AddScoped<ApplicationService>();
        builder.Services.AddTransient<OpenAiService>();
        builder.Services.AddScoped<EvaluationService>();
        builder.Services.AddScoped<AssessmentService>();
        builder.Services.AddScoped<PdfService>();
        builder.Services.AddTransient<AuthService>();

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

        var dbSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await dbSeeder.Seed();

        app.Run();
    }
}