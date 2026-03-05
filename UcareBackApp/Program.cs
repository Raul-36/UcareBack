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
using UcareBackApp.Options;
using Microsoft.Extensions.AI;
using GenerativeAI.Microsoft;
using Microsoft.Extensions.Options;
using UcareBackApp.Chats.Repositories.Base;
using UcareBackApp.Chats.Repositories;
using UcareBackApp.Chats.Services.Base;
using UcareBackApp.Chats.Services;
using Npgsql;
using UcareBackApp.Seeders;

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

var connectionString = builder.Configuration.GetConnectionString("psqlDb");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<UcareDbContext>(options =>
{
    options.UseNpgsql(dataSource);
});

builder.Services.Configure<LLMOptions>(builder.Configuration.GetSection("LLM"));

builder.Services.AddSingleton<IChatClient>(sp =>
{
    var llmOptions = sp.GetRequiredService<IOptions<LLMOptions>>().Value;
    return new GenerativeAIChatClient(llmOptions.ApiKey, llmOptions.ModelId);
});

builder.Services.InitAspnetIdentity(builder.Configuration);
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<ICardRepository, CardEfRepository>();
builder.Services.AddTransient<ICardAccessChecker, CardAccessChecker>();
builder.Services.AddTransient<ICardService, CardService>();
builder.Services.AddTransient<IChatRepository, ChatEFRepository>();
builder.Services.AddTransient<IChatService, LLMChatService>();
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedAdminAsync(services);
}

app.Run();