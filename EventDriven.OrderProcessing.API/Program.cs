using System.Text;
using System.Threading.RateLimiting;
using EventDriven.OrderProcessing.API.Endpoints;
using EventDriven.OrderProcessing.API.Middlewares;
using EventDriven.OrderProcessing.Application;
using EventDriven.OrderProcessing.Application.Auth.Interfaces;
using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
using EventDriven.OrderProcessing.Domain.Users;
using EventDriven.OrderProcessing.Infrastructure;
using EventDriven.OrderProcessing.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("login", context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddAutoMapper(cfg => { },
    typeof(OrderMappingProfile).Assembly);

builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseResponseCompression();

app.MapControllers();
app.MapOrderEndpoints();
app.MapAuthEndpoints();

//Seeding for testing purposes:
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    if (!context.Users.Any())
    {
        var admin = new User(
            email: "admin@test.com",
            passwordHash: passwordHasher.Hash("123456"),
            role: "Admin"
        );

        var user = new User(
            email: "user@test.com",
            passwordHash: passwordHasher.Hash("123456"),
            role: "User"
        );

        context.Users.AddRange(admin, user);
        await context.SaveChangesAsync();
    }
}

app.Run();
