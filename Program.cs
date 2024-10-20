

using examdb.DelayMiddleware;
using examEfCore.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddService();


var app = builder.Build();

app.UseMiddleware<CustomMiddleware>();


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();



app.Run();

