using RaceDay.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();



// Register authorization services.
builder.Services.AddAuthorization();

// Register the API HTTP client.
builder.Services.AddHttpClient("RaceDayApi", client =>
{
    var apiUrl = builder.Configuration["ApiSettings:BaseUrl"];

    client.BaseAddress = new Uri(apiUrl!);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RaceDayApiService>();


// Enable session support for login and user-role information.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();