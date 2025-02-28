using FluentMigrator.Runner;
using Smartway_task.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Todo 1 в репозиториях addpassport и тп была обработка ситуаций res=0
//Todo 2 в репе addempl была строка employee.Id = res;
//Todo 3 убрать * в sql запросах
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load("../.env");
}
var connectionString = builder.Configuration["ConnectionStrings:Database"];

builder.Services.AddSwaggerGen();
builder.Services.AddMigrations(connectionString);
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

// app.UseCors(x => x.AllowAnyMethod()
//     .AllowAnyHeader()
//     .SetIsOriginAllowed(origin => true) 
//     .AllowCredentials());
app.Run();

