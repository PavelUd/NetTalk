using System.Reflection;
using API.Converters;
using API.Hubs;
using API.Middlewares;
using Application.Common.Extensions;
using Application.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using Persistence.Contexts;
using Persistence.Extensions;
using ILogger = NLog.ILogger;



var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ILogger>(_ => LogManager.GetLogger("DefaultLogger"));
builder.Host.UseNLog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    var settings = options.SerializerSettings;
    settings.Converters.Add(new TimeOnlyJsonConverter());
    settings.Converters.Add(new DateOnlyJsonConverter());
    settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

builder.Services.Configure<Token>(builder.Configuration.GetSection("token"));
builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection(ConnectionOptions.ConfigSectionPath));
builder.Services.AddScoped<IUser, IdentityUser>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Office-Seats API",
        Description = "API"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Введите строку авторизации следующим образом: `Bearer созданный JWT Токен`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();


