using System.Reflection;
using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenLibraryClient;
using StubbedDTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


});

var SourceData = Environment.GetEnvironmentVariable("SOURCE_DATA");

switch (SourceData)
{
    case "STUB":
        builder.Services.AddSingleton<IDtoManager, Stub>();
        break;
    case "API":
        builder.Services.AddSingleton<IDtoManager, OpenLibClientAPI>();
        break;
}
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

