using api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string dbConnectionString = File.ReadAllText("dbConnectionString.txt");
builder.Services.AddSingleton<IDb>(_ => new PostgreDb(dbConnectionString));
var app = builder.Build();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();