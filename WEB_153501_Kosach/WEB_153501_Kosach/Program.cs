using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
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

//��������� ������� ��� �������� � API
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddHttpClient<IFurnitureService, ApiFurnitureService>(opt =>
                                            opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<IFurnitureCategoryService, ApiCategoryService>(opt =>
                                            opt.BaseAddress = new Uri(uriData.ApiUri));

//��������� �������� ���������������
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(opt =>
                        {
                            opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            opt.DefaultChallengeScheme = "oidc";
                        })
                        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddOpenIdConnect("oidc", options =>
                        {
                            options.Authority =
                                    builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                            options.ClientId =
                                    builder.Configuration["InteractiveServiceSettings:ClientId"];
                            options.ClientSecret =
                                    builder.Configuration["InteractiveServiceSettings:ClientSecret"];
                            // �������� Claims ������������
                            options.GetClaimsFromUserInfoEndpoint = true;
                            options.ResponseType = "code";
                            options.ResponseMode = "query";
                            options.SaveTokens = true;
                        }).AddJwtBearer(opt =>
                        {
                            opt.Authority = 
                                    builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                            opt.TokenValidationParameters.ValidateAudience = false;
                            opt.TokenValidationParameters.ValidTypes =
                                                            new[] { "at+jwt" };
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