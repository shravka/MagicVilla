using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_VillaAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();

//builder.Services.AddControllers();

builder.Services.AddSingleton<ILogging, Logging>();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
//builder.Services.AddScoped<IRepository<T>, Repository<TagHelperServicesExtensions>>();

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
