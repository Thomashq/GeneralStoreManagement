using Domain.Interfaces;
using Domain.Models;
using GeneralStoreManagement.Services;
using Infraestructure;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<BinanceService>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()    // Permite todas as origens.
              .AllowAnyHeader()   // Permite todos os cabeçalhos.
              .AllowAnyMethod();  // Permite todos os métodos HTTP.
    });
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
