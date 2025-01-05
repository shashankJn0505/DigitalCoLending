using CoLending.API.Middleware;
using CoLending.Core.Options;
using CoLending.Infrastructure.HttpService;
using CoLending.Infrastructure.SqlService;
using CoLending.Logs;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<CookieOptions>(options =>
{
    options.HttpOnly = true;
    options.Secure = true;
    options.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    options.Path = "/";
}
);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(180);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

});
builder.Services.AddHttpClient();
builder.Services.AddCors();

#region Options   
builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection(ConfigurationOptions.Configuration));
builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings));
#endregion


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Saarthi API",
        Description = "Saarthi web api"
        //TermsOfService = new Uri("https://example.com/terms"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Example Contact",
        //    Url = new Uri("https://example.com/contact")
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Example License",
        //    Url = new Uri("https://example.com/license")
        //}
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

#region Http Service
builder.Services.AddScoped<IHttpService, HttpService>();
//builder.Services.AddScoped<ADUserProdAuthMainClient>();
#endregion

#region Sql Service
builder.Services.AddScoped<ISqlUtility, SqlUtility>();
#endregion

#region Manager
#endregion

#region Repository
#endregion


builder.Services.AddControllers()
          .AddXmlSerializerFormatters();
var tokenOptions = builder.Configuration.GetSection("Auth").Get<TokenOptions>();
builder.Services.AddSingleton<ILog, Log>();
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.Secret))
    };
});

var DevelopmentOptions = builder.Configuration.GetSection("Configuration").Get<ConfigurationOptions>();
builder.Services.AddHttpContextAccessor();
if (DevelopmentOptions.VaptSetting == "0")
{
    // Configure the HTTP request pipeline.
    var app = builder.Build();
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        if (DevelopmentOptions.SwaggerEnable == 1)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
    if (app.Environment.IsDevelopment())
    {
        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    }
    else
    {
        app.UseCors(builder =>
        {
            //builder.WithOrigins(
            //    "http://localhost:4200",
            //    "http://172.26.101.56",
            //    "http://172.26.101.56:8080",
            //    "https://test.saarthi.tmf.co.in",
            //    "https://saarthi.tmf.co.in"
            //    ).AllowAnyHeader().AllowAnyMethod();

            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    }
    app.UseHttpsRedirection();
    app.UseMiddleware<GlobalErrorHandlingMiddleware>();
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseCookiePolicy();
    app.UseMiddleware<AesEncryptDecryptMiddleware>();
    app.UseAuthentication();
    app.UseSession();
    app.UseAuthorization();
    app.MapControllers();
    //app.MapWhen(context => context.Request.Path == "/api/Payment/SavePaymentStatus", m => {
    //    m.UseMiddleware<RequestResponseLoggingMiddleware>();
    //});

    app.Run();
}
else
{
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins(
                "http://localhost:4200",
                "http://172.26.101.56",
                "http://172.26.101.56:8080",
                "https://test.saarthi.tmf.co.in",
                "https://saarthi.tmf.co.in");
                          });
    });
    var app = builder.Build();
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        if (DevelopmentOptions.SwaggerEnable == 1)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
    if (app.Environment.IsDevelopment())
    {
        app.UseCors(MyAllowSpecificOrigins);
    }
    else
    {
        app.UseCors(MyAllowSpecificOrigins);
    }
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
        await next();
    });

    app.UseHttpsRedirection();
    if (DevelopmentOptions.VaptSetting == "1")
    {
        app.UseMiddleware<RefererValidationMiddleware>();
    }
    app.UseMiddleware<GlobalErrorHandlingMiddleware>();
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
    app.UseCookiePolicy();
    app.UseMiddleware<AesEncryptDecryptMiddleware>();
    app.UseAuthentication();
    app.UseSession();
    app.UseAuthorization();
    app.MapControllers();


    app.Run();
}
