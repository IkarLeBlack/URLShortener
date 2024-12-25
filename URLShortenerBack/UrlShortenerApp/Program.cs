using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Hub;
using URL_Shortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy => 
    {
        policy.WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
    });
});


builder.Services.AddScoped<UserService>();  
builder.Services.AddScoped<UrlService>();   
builder.Services.AddSignalR(); 

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseCors("AllowAngularApp");
app.UseRouting();
app.UseAuthorization();
app.MapHub<UrlUpdateHub>("/urlUpdateHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapFallbackToFile("/angular/URLShortner/src/index.html");
app.Run();
