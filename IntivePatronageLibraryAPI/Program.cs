using System.Net.Mime;
using IntivePatronageLibraryAPI.Mapping;
using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Services;
using IntivePatronageLibraryDATA;
using IntivePatronageLibrarySERVICES;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LibraryDbContext = IntivePatronageLibraryDATA.LibraryDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<LibraryDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDBSqlConnectionPCSQLSERVER"), x=> x.MigrationsAssembly("IntivePatronageLibraryDATA")));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseAuthorization();

app.MapControllers();

app.Run();