using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Hospital.Application;
using Hospital.Infrastructure;
using Hospital.Infrastructure.Contexts;
using Hospital.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<IHospitalDbContext,HospitalContext>(opt =>
    {
        opt.UseInMemoryDatabase("disigntime.db");
    });
}
else
{
    builder.Services.AddDbContext<IHospitalDbContext, HospitalContext>(opt =>
    {
        opt.UseSqlServer("Data Source=host.docker.internal,1433;Initial Catalog=MyDB;User ID=MyUser;Password=MyPassword");
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
