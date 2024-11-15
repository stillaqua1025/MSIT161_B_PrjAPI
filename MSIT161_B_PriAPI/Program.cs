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

// �]�m���ε{������|�å[�� appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())  // �]�w����|
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // �[�� appsettings.json
    .AddEnvironmentVariables();

//���UDI
builder.Services.AddDbContext<dbMSTI161_B_ProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbMSTI161_B_Project")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbMSTI161_B_Project")));
builder.Services.AddHttpClient();

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

// �t�m Identity �M JWT ����
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("Jwt");

// ���U IMemoryCache
builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;  // �b�}�o�ɤ��\ HTTP�A�����������ӱҥ� HTTPS
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();  // �}�o�Ҧ��U���\ HTTP�A�Ͳ��Ҧ������ϥ� HTTPS
    options.SaveToken = true;  // �O�s Token �� HttpContext
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // ���ҵo���
        ValidateAudience = true,  // ���ұ�����
        ValidateLifetime = true,  // ���� Token ���Ĵ�
        ValidateIssuerSigningKey = true,  // ����ñ�W�K�_
        ValidIssuer = jwtSettings["Issuer"],  // �o���
        ValidAudience = jwtSettings["Audience"],  // ������
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),  // ñ�W�K�_
        ClockSkew = TimeSpan.Zero  // ����ɶ��}���A�]�w��0�H���T������Token
    };

    // �K�[�ƥ�B�z�Ӯ������ҹL�{�������~
    options.Events = new JwtBearerEvents
    {
        //OnMessageReceived = context =>
        //{
        //    var accessToken = context.Request.Query["access_token"];

        //    // �p�G�ШD�O�Ӧ� SignalR hub
        //    var path = context.HttpContext.Request.Path;
        //    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
        //    {
        //        context.Token = accessToken;  // �q QueryString ������ Token
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

builder.Services.AddDistributedMemoryCache(); // �ϥΤ��s�@��Session�x�s
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // �]�mSession�L���ɶ�
    options.Cookie.HttpOnly = true; // Session�u���\��ݳX��
    options.Cookie.IsEssential = true; // Session������ε{���O���n��
});

// �]�w CORS(���)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins("http://localhost:4200")  // �]�m���\���e�����Ψӷ�
               .AllowAnyMethod()  // ���\�Ҧ� HTTP ��k
               .AllowAnyHeader()  // ���\�Ҧ����Y
               .AllowCredentials());  // ���\�a�� Authorization ���Y���ШD
    //options.AddPolicy("AllowAll", builder =>
    //    builder.AllowAnyOrigin()  // ���\�Ҧ��ӷ�
    //           .AllowAnyMethod()  // ���\�Ҧ� HTTP ��k
    //           .AllowAnyHeader());  // ���\�Ҧ����Y
});

// ���U IHttpContextAccessor �M JwtService
builder.Services.AddHttpContextAccessor();  // ���U IHttpContextAccessor
builder.Services.AddScoped<JwtService>();  // ���U JwtService


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// ���U EmailSender �� IEmailSender �������{
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "��L�m�A���ʪ����xAPI",  // �۩w�q API �W��
        Version = "1.0",  // �۩w�q API ������
        Description = "�o�O��L�m�A���ʪ����x�� API ���A�Ѷ}�o�̨ϥΡC",  // �A�i�H�K�[�y�z
        Contact = new OpenApiContact
        {
            Name = "MSIT161_B",
            Email = "one2clothesplatform@gmail.com",
            Url = new Uri("http://localhost:4200/")
        }
    });

    // �w�q Bearer Token �{��
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "��L�m�A���ʪ����xAPI v1");
    c.RoutePrefix = "swagger";  // �NSwagger UI���ѳ]�m�� /swagger
});

// �]�w HTTP �ШD�޽u
if (app.Environment.IsDevelopment())
{
    //��ɭԤW��swagger���i��
    app.UseDeveloperExceptionPage(); // �T�O�b�}�o�Ҧ��U��ܸԲӪ����`�H��
}

app.UseStaticFiles();  // �ҥ��R�A�ɮתA��
//�}�o���Ҥ����Q�j��ϥ� HTTPS�A�i�H�ȮɸT��
//app.UseHttpsRedirection();//�u���\ HTTPS �X��

//app.UseCors("AllowAll");  // �ҥ� CORS �F��
app.UseCors("AllowSpecificOrigin");  // �ϥΨ���t�m�� CORS �F��

app.UseSession();

app.UseAuthentication();  // �T�O�o��b UseAuthorization ���e�AJWT ���ҷ|�b���B�i��
app.UseAuthorization();  // ���v�ШD

app.MapControllers();  // �t�m�������

//
app.MapHub<ChatHub>("/chat-hub");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
