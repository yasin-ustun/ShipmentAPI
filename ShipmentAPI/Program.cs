using ShipmentAPI.Repositories;
using ShipmentAPI.Repositories.Interfaces;
using ShipmentAPI.Services;
using ShipmentAPI.Services.Interfaces;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUnitOfService, UnitOfService>();
builder.Services.AddTransient<SqlConnection>(option => new SqlConnection(builder.Configuration.GetConnectionString("AppConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
