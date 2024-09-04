using Hospital.API.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApiMainContext>(opt =>
    {
        opt.UseInMemoryDatabase("disigntime.db");
    }); 
}
else
{
    builder.Services.AddDbContext<ApiMainContext>(opt =>
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
