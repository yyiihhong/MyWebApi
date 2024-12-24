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
// �]�w JWT ����
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
    // �t�m Swagger ���ɰ򥻫H��
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // �t�m Customer �������M�g�]�i��^
    c.MapType<IEnumerable<Customer>>(() => new OpenApiSchema
    {
        Type = "array",
        Items = new OpenApiSchema { Type = "object" }
    });

    // �t�m Bearer Token �{�Ҥ覡
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, // �b�ШD�����Y���ǻ� token
        Description = "�п�J Bearer Token", // ���ܤ�r
        Name = "Authorization", // ���Y�W��
        Type = SecuritySchemeType.ApiKey // �w�q�� API �K�_
    });

    // �t�m�Ҧ����I�ݭn Bearer Token ����
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // �ϥΫe���w�q�� Bearer ���
                }
            },
            new string[] {} // �L���B�~������νd��
        }
    });
});


// �[�J MSSQL �䴩
builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ���U�̩ۨ�
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
