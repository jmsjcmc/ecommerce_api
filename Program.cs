using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ecommerce_api.Data;
using ecommerce_api.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();
// Learn more about configuring Swagger/OpenAPI JsonReaderException: '"' is invalid after a value. Expected either ',', '}', or ']'. LineNumber: 11 | BytePositionInLine: 2.at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

app.Run();
