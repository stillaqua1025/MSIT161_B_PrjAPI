using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MSIT161_B_PriAPI.Hubs;
using MSIT161_B_PriAPI.Models;
using MSIT161_B_PriAPI.Providers;
using MSIT161_B_PriAPI.Repositories;
using prj_MSIT161_B.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 設置應用程式基路徑並加載 appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())  // 設定基路徑
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // 加載 appsettings.json
    .AddEnvironmentVariables();

//註冊DI
builder.Services.AddDbContext<dbMSTI161_B_ProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbMSTI161_B_Project")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbMSTI161_B_Project")));
builder.Services.AddHttpClient();

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

// 配置 Identity 和 JWT 驗證
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("Jwt");

// 註冊 IMemoryCache
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;  // 在開發時允許 HTTP，正式環境應該啟用 HTTPS
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();  // 開發模式下允許 HTTP，生產模式必須使用 HTTPS
    options.SaveToken = true;  // 保存 Token 到 HttpContext
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // 驗證發行者
        ValidateAudience = true,  // 驗證接收者
        ValidateLifetime = true,  // 驗證 Token 有效期
        ValidateIssuerSigningKey = true,  // 驗證簽名密鑰
        ValidIssuer = jwtSettings["Issuer"],  // 發行者
        ValidAudience = jwtSettings["Audience"],  // 接收者
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),  // 簽名密鑰
        ClockSkew = TimeSpan.Zero  // 防止時間漂移，設定為0以更精確的驗證Token
    };

    // 添加事件處理來捕獲驗證過程中的錯誤
    options.Events = new JwtBearerEvents
    {
        //OnMessageReceived = context =>
        //{
        //    var accessToken = context.Request.Query["access_token"];

        //    // 如果請求是來自 SignalR hub
        //    var path = context.HttpContext.Request.Path;
        //    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
        //    {
        //        context.Token = accessToken;  // 從 QueryString 中提取 Token
        //    }

        //    return Task.CompletedTask;
        //},
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Token validation failed: {context.Exception.Message}");
            Console.WriteLine($"StackTrace: {context.Exception.StackTrace}");
            Console.WriteLine($"Received Token: {context.Request.Headers["Authorization"]}");
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddDistributedMemoryCache(); // 使用內存作為Session儲存
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 設置Session過期時間
    options.Cookie.HttpOnly = true; // Session只允許後端訪問
    options.Cookie.IsEssential = true; // Session對於應用程式是必要的
});

// 設定 CORS(跨域)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins("http://localhost:4200")  // 設置允許的前端應用來源
               .AllowAnyMethod()  // 允許所有 HTTP 方法
               .AllowAnyHeader()  // 允許所有標頭
               .AllowCredentials());  // 允許帶有 Authorization 標頭的請求
    //options.AddPolicy("AllowAll", builder =>
    //    builder.AllowAnyOrigin()  // 允許所有來源
    //           .AllowAnyMethod()  // 允許所有 HTTP 方法
    //           .AllowAnyHeader());  // 允許所有標頭
});

// 註冊 IHttpContextAccessor 和 JwtService
builder.Services.AddHttpContextAccessor();  // 註冊 IHttpContextAccessor
builder.Services.AddScoped<JwtService>();  // 註冊 JwtService


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// 註冊 EmailSender 為 IEmailSender 的具體實現
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "衣貳衫服飾購物平台API",  // 自定義 API 名稱
        Version = "1.0",  // 自定義 API 版本號
        Description = "這是衣貳衫服飾購物平台的 API 文件，供開發者使用。",  // 你可以添加描述
        Contact = new OpenApiContact
        {
            Name = "MSIT161_B",
            Email = "one2clothesplatform@gmail.com",
            Url = new Uri("http://localhost:4200/")
        }
    });

    // 定義 Bearer Token 認證
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<NotificationService>();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "衣貳衫服飾購物平台API v1");
    c.RoutePrefix = "swagger";  // 將Swagger UI路由設置為 /swagger
});

// 設定 HTTP 請求管線
if (app.Environment.IsDevelopment())
{
    //到時候上面swagger移進來
    app.UseDeveloperExceptionPage(); // 確保在開發模式下顯示詳細的異常信息
}

app.UseStaticFiles();  // 啟用靜態檔案服務
//開發環境中不想強制使用 HTTPS，可以暫時禁用
//app.UseHttpsRedirection();//只允許 HTTPS 訪問

//app.UseCors("AllowAll");  // 啟用 CORS 政策
app.UseCors("AllowSpecificOrigin");  // 使用具體配置的 CORS 政策

app.UseSession();

app.UseAuthentication();  // 確保這行在 UseAuthorization 之前，JWT 驗證會在此處進行
app.UseAuthorization();  // 授權請求

app.MapControllers();  // 配置控制器路由

//
app.MapHub<ChatHub>("/chat-hub");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
