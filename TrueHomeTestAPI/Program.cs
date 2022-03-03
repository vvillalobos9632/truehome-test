using Common.DTOs.Settings;
using Common.Extensions.Utils;
using IServices.TrueHome;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Presentation.CommonSettings;
using Services.TrueHome;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(option =>
              {
                  option.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateIssuer = CommonSettings.TokenValidationParameters.ValidateIssuer,
                      ValidateAudience = CommonSettings.TokenValidationParameters.ValidateAudience,
                      ValidateLifetime = CommonSettings.TokenValidationParameters.ValidateLifetime,
                      ValidateIssuerSigningKey = CommonSettings.TokenValidationParameters.ValidateIssuerSigningKey,
                      ValidIssuer = CommonSettings.TokenValidationParameters.ValidIssuer,
                      ValidAudience = CommonSettings.TokenValidationParameters.ValidAudience,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CommonSettings.TokenValidationParameters.SecurityKey)),
                      ClockSkew = TimeSpan.Zero,
                  };
              });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy(CommonSettings.PolicyRules, builder =>
          builder.AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowAnyOrigin()));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<IConnectionStringsSettings, ConnectionStringsSettings>();
InsertServicesDependencyInjection(builder);
StartupRepositoriesDependency.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

app.UseCors(builder =>
{
    builder.WithOrigins(CommonSettings.AllowedOriginsLocal)
    .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/html";
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is FriendlyException)
            context.Response.StatusCode = 400;

        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
    });
});
app.UseHsts();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
void InsertServicesDependencyInjection(WebApplicationBuilder builder)
{
    var interfacesTypes = typeof(IUserService).GetAllInterfaces();

    var implementationTypes = typeof(UserService).GetAllImplementations();

    foreach (var interfaceType in interfacesTypes)
        foreach (var implementationType in implementationTypes.Where(impType => interfaceType.IsAssignableFrom(impType)))
            builder.Services.AddTransient(interfaceType, implementationType);

}