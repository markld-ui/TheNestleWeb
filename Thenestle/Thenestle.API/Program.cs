using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Thenestle.Application.Servicies.Auth;
using Thenestle.Domain.Interfaces.Auth;
using Thenestle.Persistence.Data;
using Thenestle.Domain.Interfaces.Repositories;
using Thenestle.Persistence.Repositories;
using Thenestle.Domain.Interfaces.Services;
using Thenestle.Application.Servicies.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICoupleRepository, CoupleRepository>();
builder.Services.AddScoped<IInviteRepository, InviteRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IGeneratorCode, GeneratorCode>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Введите JWT токен без префикса 'Bearer '",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "API для обычных пользователей"
    });

    c.SwaggerDoc("v99", new OpenApiInfo
    {
        Title = "Admin API",
        Version = "v99",
        Description = "API для администраторов"
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"])),
        ClockSkew = TimeSpan.FromMinutes(5),
        NameClaimType = "sub"
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Явно берем токен из заголовка
            context.Token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "Ошибка аутентификации JWT");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("JWT токен успешно валидирован");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5205", "https://localhost:5205", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.SetMinimumLevel(LogLevel.Information);
//builder.Logging.AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug);
//builder.Logging.AddFilter("Microsoft.AspNetCore.Authentication.JwtBearer", LogLevel.Debug);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thenestle User API v1");
        c.SwaggerEndpoint("/swagger/v99/swagger.json", "Thenestle Admin API v99");

        c.ConfigObject.DisplayOperationId = true;
        c.ConfigObject.DisplayRequestDuration = true;
        c.ConfigObject.DocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List;

        c.ConfigObject.DefaultModelRendering =
            Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model;
    });
}

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    Console.WriteLine($"Method: {context.Request.Method}");
    Console.WriteLine($"Path: {context.Request.Path}");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }

    // Log all headers
    foreach (var header in context.Request.Headers)
    {
        logger.LogInformation("Header: {Key}: {Value}", header.Key, header.Value);
    }

    // Specifically log the Authorization header
    if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
    {
        logger.LogInformation("Authorization header: {Value}", authHeader);
    }
    else
    {
        logger.LogWarning("No Authorization header present");
    }

    //if (context.Request.Method == "OPTIONS")
    //{
    //    var origin = context.Request.Headers["Origin"].ToString();
    //    if (origin == "http://localhost:5173" || origin == "http://localhost:5205")
    //    {
    //        context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
    //        context.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type");
    //        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
    //        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
    //        context.Response.StatusCode = 200;
    //        await context.Response.CompleteAsync();
    //        return;
    //    }
    //}


    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

try
{
    if (await dbContext.Database.CanConnectAsync())
    {
        Console.WriteLine("Успешное подключение к базе данных!");
    }
    else
    {
        Console.WriteLine("Не удалось подключиться к базе данных.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
}

app.Run();