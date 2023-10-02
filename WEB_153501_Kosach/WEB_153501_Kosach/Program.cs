using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WEB_153501_Kosach;
using WEB_153501_Kosach.Domain.Entities;
using WEB_153501_Kosach.Services.FurnitureCategoryService;
using WEB_153501_Kosach.Services.FurnitureServices;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IFurnitureService, ApiFurnitureService>();
builder.Services.AddScoped<IFurnitureCategoryService, ApiCategoryService>();

//Внедрение клиента для запросов к API
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddHttpClient<IFurnitureService, ApiFurnitureService>(opt =>
                                            opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<IFurnitureCategoryService, ApiCategoryService>(opt =>
                                            opt.BaseAddress = new Uri(uriData.ApiUri));

//Внедрение сервисов аунтентификации
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(opt =>
                        {
                            opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            opt.DefaultChallengeScheme = "oidc";
                        })
                        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddJwtBearer(opt =>
                        {
                            opt.Authority = 
                                    builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                            opt.TokenValidationParameters.ValidateAudience = false;
                            opt.TokenValidationParameters.ValidTypes =
                                                            new[] { "at+jwt" };
                            opt.TokenValidationParameters.RoleClaimType = "role";
                        })
                        .AddOpenIdConnect("oidc", options =>
                        {
                            options.Authority =
                                    builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                            options.ClientId =
                                    builder.Configuration["InteractiveServiceSettings:ClientId"];
                            options.ClientSecret =
                                    builder.Configuration["InteractiveServiceSettings:ClientSecret"];

                            // Получить Claims пользователя
                            options.GetClaimsFromUserInfoEndpoint = true;
                            options.ResponseType = "code";
                            options.ResponseMode = "query";
                            options.SaveTokens = true;

                            

                            options.Scope.Add("role");
                            options.ClaimActions.MapJsonKey("role", "role", "role");
                            options.Scope.Add("api_role");
                            options.ClaimActions.MapJsonKey("api_role", "role", "role");
                            //options.ClaimActions.Add(new JsonKeyClaimAction("api_role", null, "role"));
                            options.TokenValidationParameters.RoleClaimType = "role";
                        });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseCookiePolicy(new CookiePolicyOptions()
//{
//    MinimumSameSitePolicy = SameSiteMode.Lax
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();