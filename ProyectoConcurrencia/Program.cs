
using Microsoft.AspNetCore.Mvc;
using ProyectoConcurrencia.RabbitMq;
using ProyectoConcurrencia.Validations;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<receiveBranchesData>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

readFile readFile = new readFile();
Validations validate = new Validations();
app.MapControllers();
readFile.readCsvFile();
readFile.readFromList();
validate.checkList();
app.Run();

