using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UcareBackApp.Data;
using UcareBackApp.Extensions;
using UcareBackApp.Repositories;
using UcareBackApp.Cards.Repositories.Base;
using UcareBackApp.Cards.Services.Base;
using UcareBackApp.Cards.Services;
using UcareBackApp.Services.ImageService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

builder.Services.AddEndpointsApiExplorer();
builder.Services.InitSwagger();
builder.Services.AddDbContext<UcareDbContext>(options =>
{
    var connectinoString = builder.Configuration.GetConnectionString("psqlDb");
    options.UseNpgsql(connectinoString);
});
builder.Services.Add
builder.Services.InitAspnetIdentity(builder.Configuration);
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<ICardRepository, CardEfRepository>();
builder.Services.AddTransient<ICardAccessChecker, CardAccessChecker>();
builder.Services.AddTransient<ICardService, CardService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UcareDbContext>();
    context.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider($@"{AppDomain.CurrentDomain.BaseDirectory}/wwwroot")
});

app.UseAuthorization();

app.MapControllers();


app.Run();