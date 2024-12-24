using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyWebApi.Interfaces.Repositories;
using MyWebApi.Interfaces.Services;
using MyWebApi.Models;
using MyWebApi.Repositories;
using MyWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
// 設定 JWT 驗證
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 配置 Swagger 文檔基本信息
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // 配置 Customer 類型的映射（可選）
    c.MapType<IEnumerable<Customer>>(() => new OpenApiSchema
    {
        Type = "array",
        Items = new OpenApiSchema { Type = "object" }
    });

    // 配置 Bearer Token 認證方式
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, // 在請求的標頭中傳遞 token
        Description = "請輸入 Bearer Token", // 提示文字
        Name = "Authorization", // 標頭名稱
        Type = SecuritySchemeType.ApiKey // 定義為 API 密鑰
    });

    // 配置所有端點需要 Bearer Token 驗證
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // 使用前面定義的 Bearer 方案
                }
            },
            new string[] {} // 無需額外的角色或範圍
        }
    });
});


// 加入 MSSQL 支援
builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// 註冊相依性
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
