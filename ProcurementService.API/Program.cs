using ProcurementService.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProcurementService.API.Service.Registeration;
using ProcurementService.API.Service;
using ProcurementService.API.DAL.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.LogTo(Console.WriteLine);
    options.UseSqlServer(builder.Configuration["AppSettings:ConnectionString"]);  
});

builder.Services.AddUnitOfWork();

builder.Services.AddRepositories();

builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

app.MigrateApplicationAsync();

app.UseExceptionHandler(_ => { });

app.Map("*", (IOptions<AppSettings> options) =>
{
    AppSettings _appSettings = options.Value;

    _appSettings.DirectoryFiles = $"{AppDomain.CurrentDomain.BaseDirectory}{_appSettings.DirectoryFiles}";
    
    return _appSettings;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(options => options
    .WithOrigins(new[] { "http://localhost:3000", "http://localhost:8080" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
