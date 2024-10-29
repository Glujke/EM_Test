using EM_TestRepository.Context;
using EM_TestRepository.Entity;
using EM_TestRepository.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EM Test API", Version = "v1" });
});

builder.Services.AddDbContext<EM_TestContext>(options =>
options.UseSqlite("Data Source=EM_Test.db"));

builder.Services.AddScoped<IRepository<Location>, RepositoryBase<Location>>();
builder.Services.AddScoped<IRepository<Order>, RepositoryOrder>();
builder.Services.AddScoped<ISortable<Order>, RepositoryOrder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EM Test API V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
