using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using ToBeApi.Data;
using ToBeApi.Extensions;
using ToBeApi.Extensions.Middleware;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using ToBeApi.Filters;
using ToBeApi.Data.Shaper;
using ToBeApi.Data.DTO;
using ToBeApi.Utility;
using AspNetCoreRateLimit;
using ToBeApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
   .AddConfiguration(configuration));


ILogger logger = loggerFactory.CreateLogger<Program>();


// Add services to the container.

builder.Services.AddSingleton(typeof(ILogger), logger);

builder.Services.ConfigureSqlContext(configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureVersioning();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRareLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateExistsAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

builder.Services.AddScoped<IDataShaper<PostDTO>, DataShaper<PostDTO>>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<PostLinks>();
builder.Services.AddControllers(configuration =>
{
    configuration.RespectBrowserAcceptHeader = true;
    configuration.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.ConfigureExceptionHandler(logger);

app.UseIpRateLimiting();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "ToBe Api v1");
});
app.MapControllers();

app.Run();
