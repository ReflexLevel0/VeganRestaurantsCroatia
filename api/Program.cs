using System.Reflection;
using api;
using api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SupportNonNullableReferenceTypes();
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});
string dbConnectionString = File.ReadAllText("dbConnectionString.txt");
builder.Services.AddSingleton<IDb>(_ => new PostgreDb(dbConnectionString));
var app = builder.Build();
app.Map("/error", () => new ApiResponseWrapper("Internal Server Error", "Something broke in the code :(", null));
app.Map("/{*path}", () => new ApiResponseWrapper("Not Found", "API path doesn't exist", null));
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler("/error");
app.Run();