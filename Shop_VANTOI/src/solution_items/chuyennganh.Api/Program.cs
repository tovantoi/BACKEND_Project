using chuyennganh.Api.Endpoints;
using chuyennganh.Application.App.JWT;
using chuyennganh.Application.Extensions;
using chuyennganh.Domain.DependencyInjection.Extensions;
using chuyennganh.Domain.Middlewares;
using chuyennganh.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Bind JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

// Validate JwtSettings khi startup
if (jwtSettings == null ||
    string.IsNullOrEmpty(jwtSettings.Issuer) ||
    string.IsNullOrEmpty(jwtSettings.Audience) ||
    string.IsNullOrEmpty(jwtSettings.Key))
{
    throw new InvalidOperationException("Cấu hình JwtSettings trong appsettings.json bị thiếu hoặc sai. Vui lòng kiểm tra lại.");
}

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

Console.WriteLine("Issuer: " + builder.Configuration["JwtSettings:Issuer"]);
Console.WriteLine("Audience: " + builder.Configuration["JwtSettings:Audience"]);
Console.WriteLine("Key: " + builder.Configuration["JwtSettings:Key"]);

// C?u hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Add persistence services
builder.Services.AddInfractureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Environment.AddEnvironmentHelper();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddConfigSetting(builder.Configuration);
// Add application services
builder.Services.AddApplicationServices();
builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();
app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    // Cho phép tất cả origin – bạn có thể giới hạn theo domain thật sau
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

    // Nếu là request OPTIONS (preflight), trả về sớm
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

app.UseStaticFiles();

// Bắt buộc: Routing → CORS → Authentication → Authorization → Endpoints
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts();
}



// ??nh tuy?n các endpoint cho controllers
app.MapProductEndpoints();
app.MapCategoryEndpoints();
app.MapCustomerEndpoints();
app.MapOrderEndpoints();
app.MapControllers();
app.MapCouponEndpoints();
app.MapCustomerAddressEndpoints();
app.MapProductReviewEndpoints();
app.MapPaymentEndpoints();
app.MapBlogEndpoints();
app.MapThongKeEndpoints();
app.Run();
