using Amazon.S3;
using Api.Extensions;
using Api.Middleware;
using Application.Admin.Mapping;
using Application.Converters;
using Application.Hubs;
using Application.Interfaces;
using Application.Interfaces.Admin;
using Application.Mapper;
using Application.ScheduledTask;
using Application.Services;
using Application.Services.Admin;
using Infrastructure.Admin.Data;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Admin;
using Infrastructure.Services; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register IDbConnection with a concrete implementation
builder.Services.AddTransient<IDbConnection>(sp =>
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly, typeof(AdminMappingProfile).Assembly);

// Add services to the container.
builder.Services.AddScoped(typeof(IBaseRepository<, >), typeof(BaseRepository<, >));
builder.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(EFCoreUserRepository));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IMediaRepository), typeof(MediaRepository));
builder.Services.AddScoped(typeof(IMediaService), typeof(MediaService));
builder.Services.AddScoped(typeof(IPostRepository), typeof(PostRepository));
builder.Services.AddScoped(typeof(IPostService), typeof(PostService));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(INotificationRepository), typeof(NotificationRepository));
builder.Services.AddScoped(typeof(INotificationService), typeof(NotificationService));
builder.Services.AddScoped(typeof(IContractService), typeof(ContractService));
builder.Services.AddScoped(typeof(IContractRepository), typeof(ContractRepository));
builder.Services.AddScoped(typeof(IClientReviewMcService), typeof(ClientReviewMcService));
builder.Services.AddScoped(typeof(IClientReviewMcRepository), typeof(ClientReviewMcRepository));
builder.Services.AddScoped(typeof(IMcReviewClientService), typeof(McReviewClientService));
builder.Services.AddScoped(typeof(IMcReviewClientRepository), typeof(McReviewClientRepository));
builder.Services.AddScoped(typeof(IUserIdVerificationService), typeof(UserIdVerificationService));
builder.Services.AddScoped(typeof(IUserIdVerificationRepository), typeof(UserIdVerificationRepository));
builder.Services.AddScoped(typeof(IIdInfoRepository), typeof(IdInfoRepository));
builder.Services.AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));

//admin services
builder.Services.AddScoped(typeof(IAdminBaseRepository<>), typeof(AdminBaseRepository<>));
builder.Services.AddScoped(typeof(IAdminBaseService<,>), typeof(AdminBaseService<,>));
builder.Services.AddScoped(typeof(IAdminUserService), typeof(AdminUserService));

//signalR
builder.Services.AddSignalR();

// scheduled task
builder.Services.AddHostedService<ReviewBackgroundService>();

// Add CORS services.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new UtcToServerLocalTimeConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton(sp =>
{
    var s3Client = sp.GetRequiredService<IAmazonS3>();
    var bucketName = builder.Configuration["AWS:BucketName"] ?? string.Empty;
    return new S3Service(s3Client, bucketName) as IS3Service;
});

builder.Services.AddAuthenticationServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });
builder.Services.AddScoped<ITokenService, TokenService>();

//admin:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);

// Register AdminDbContext
builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseMySql(
        connectionString,
        serverVersion,
        b => b.MigrationsAssembly("Infrastructure")
    )
);

// Register AdminDbContext as base DbContext for AdminBaseRepository
builder.Services.AddScoped<DbContext, AdminDbContext>();

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        serverVersion,
        b => b.MigrationsAssembly("Infrastructure")
    )
);

var app = builder.Build();

// Use the CORS policy.
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom middleware for exception handling

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseJwtAuthentication();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");

// ****** ADD THE HEALTH CHECK ENDPOINT HERE ******
app.MapGet("/api/health", () =>
{
    // You can return a simple anonymous object or just Results.Ok()
    return Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
    // Or even simpler for just a 200 OK:
    // return Results.Ok();
});

app.Run();

public partial class Program { }
