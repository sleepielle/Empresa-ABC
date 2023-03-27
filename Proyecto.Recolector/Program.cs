
using Microsoft.AspNetCore.Mvc;
using Proyecto.Recolector.Services;
[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Recolector");
Recolector recolector = new Recolector();
recolector.SendSales();

app.Run();
