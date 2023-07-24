using BooksAPI.DataLayer.Abstractions;
using BooksAPI.DataLayer.Abstractions.Repositories;
using BooksAPI.DataLayer.EF;
using BooksAPI.DataLayer.EF.Repositories;
using BooksAPI.Services.BooksEditor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<BooksContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IBookEditor, DefaultBookEditor>();
builder.Services.AddScoped<IBookRepository, EFBooksRepository>();
builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
