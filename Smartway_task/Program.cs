using FluentMigrator.Runner;
using Smartway_task.Extensions;
using Smartway_task.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();
builder.Services.AddMigrations(builder.Configuration);
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddDapper();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();

var app = builder.Build();
var serviceProvider = app.Services.CreateScope().ServiceProvider;
var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI();    
}

app.Run();