using IntivePatronageLibraryAPI;
using IntivePatronageLibraryAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<LibraryDbContext>(option =>
//    option.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDBSqlConnection")));
builder.Services.AddDbContext<LibraryDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDBSqlConnectionPC")));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Settings.ConnectionString = builder.Configuration.GetConnectionString("LibraryDBSqlConnectionPC");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();