using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WEB_153501_Kosach.API.Data;
using WEB_153501_Kosach.API.Services;
using WEB_153501_Kosach.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectingString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(
                                    options => options.UseSqlite(connectingString));

//Регистрация сервисов аунтификации
//builder.Services.AddAuthorization();
builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = builder
                                .Configuration
                                .GetSection("isUri").Value;
                opt.TokenValidationParameters.ValidateAudience = false;
                opt.TokenValidationParameters.ValidTypes =
                                                new[] { "at+jwt" };
            });

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IFurnitureCategoryService, FurnitureCategoryService>();
builder.Services.AddScoped<IFurnitureService, FurnitureService>();

var app = builder.Build();

//DbInitializercs.SeedData(app);
//using var scope = app.Services.CreateScope();
//var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//// Выполнение миграций
//await context.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
