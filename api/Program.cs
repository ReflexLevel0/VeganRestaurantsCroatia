using api;
using api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy => policy.WithOrigins("*"));
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SupportNonNullableReferenceTypes();
	// var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	// var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	// options.IncludeXmlComments(xmlPath);
});
string dbConnectionString = File.ReadAllText("dbConnectionString.txt");
var db = new PostgreDb(dbConnectionString);
builder.Services.AddSingleton<IDb>(_ => db);
builder.Services.AddSingleton<FileHelper>(_ => new FileHelper(db));
var app = builder.Build();
app.Map("/error", () => new ApiResponseWrapper("Internal Server Error", "Something broke in the code :(", null));
app.Map("/{*path}", () => new ApiResponseWrapper("Not Found", "API path doesn't exist", null));
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseExceptionHandler("/error");
app.Run();