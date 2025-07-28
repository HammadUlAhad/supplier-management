using SupplierManagementClient.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure API settings
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));

builder.Services.Configure<DemoCredentials>(
    builder.Configuration.GetSection("DemoCredentials"));

// Add HTTP client for API calls
builder.Services.AddHttpClient("SupplierManagementApi", client =>
{
    var apiSettings = builder.Configuration.GetSection(ApiSettings.SectionName).Get<ApiSettings>();
    client.BaseAddress = new Uri(apiSettings?.BaseUrl ?? "http://localhost:5114");
    client.Timeout = TimeSpan.FromSeconds(apiSettings?.TimeoutSeconds ?? 30);
    client.DefaultRequestHeaders.Add("User-Agent", "SupplierManagementClient/1.0");
});

// Add logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Add memory cache for token storage
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
