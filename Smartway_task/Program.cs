using FluentMigrator.Runner;
using Smartway_task.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load("../.env");
}


builder.Services.AddSwaggerGen();
builder.Services.AddMigrations(builder.Configuration);
builder.Services.AddDapper();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();

var app = builder.Build();
var serviceProvider = app.Services.CreateScope().ServiceProvider;
var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

// app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.MapSwagger();
app.UseSwaggerUI();

app.Run();

