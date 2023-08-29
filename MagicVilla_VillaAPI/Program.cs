using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));
var key = builder.Configuration.GetValue<string>("ApiSetting:Secret");
//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(x => {
//        x.RequireHttpsMetadata = false;
//        x.SaveToken = true;
//        x.Authority = "http://localhost:16702/";
//        x.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    });


// this ensures Header Accept=application/json is only allowed

builder.Services.AddControllers(option =>{
  //  option.ReturnHttpNotAcceptable = true; //allows plain /text to accept as well on commenting
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddSingleton<ILogging, Logging>();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
//builder.Services.AddScoped<IRepository<T>, Repository<TagHelperServicesExtensions>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
