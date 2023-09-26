using System.Reflection;
using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MyLibraryManager;
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
    case "MYDBSTUB":
        builder.Services.AddSingleton<IDtoManager, MyLibraryMgr>();
        break;
    case "MYDBMARIA":
        string DBDatabase = Environment.GetEnvironmentVariable("MARIADB_DATABASE", EnvironmentVariableTarget.Process);
        string DBUser = Environment.GetEnvironmentVariable("MARIADB_USER", EnvironmentVariableTarget.Process);
        string DBPassword = Environment.GetEnvironmentVariable("MARIADB_PASSWORD", EnvironmentVariableTarget.Process);
        string DBPath = Environment.GetEnvironmentVariable("DBPath");
        string ConnectionString = $"server={DBPath};port=3306;database={DBDatabase};user={DBUser};password={DBPassword}";
        builder.Services.AddSingleton<IDtoManager, MyLibraryMgr>(provider => new MyLibraryMgr(DBPath));
        break;
    default:
        builder.Services.AddSingleton<IDtoManager, Stub>();
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

