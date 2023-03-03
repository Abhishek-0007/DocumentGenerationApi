using DocumentGenerationApi.DAL.DbContexts;
using DocumentGenerationApi.DAL.Repositories.Implementations;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Services.Implementations;
using DocumentGenerationApi.Services.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConnString")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<ILogService, LogService>();

builder.Services.AddHangfireServer();
builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("ConnString"));
    x.UseFilter(new AutomaticRetryAttribute { Attempts = 5 });
});




builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
