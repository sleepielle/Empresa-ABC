
using Microsoft.AspNetCore.Mvc;
using Proyecto.Gateway.Services;
//using Proyecto.Concurrencia.RabbitMq;
//using Proyecto.Concurrencia.Validations;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Transaction>();
//builder.Services.AddHostedService<receiveBranchesData>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "Gateway");
app.MapControllers();
app.Run();
