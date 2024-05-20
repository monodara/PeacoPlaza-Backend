using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;
using Server.Infrastructure.src.Middleware;
using Server.Infrastructure.src.Repo;
using Server.Infrastructure.src.Service;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.ServiceImplement.AuthServiceImplement;
using Server.Service.src.ServiceImplement.EntityServiceImplement;
using Server.Service.src.Shared;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add all controllers
builder.Services.AddControllers();
// .AddJsonOptions(options =>
//         {
//             options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//         });
builder.Services.AddHttpContextAccessor();
// Add logging configuration
builder.Logging.AddConsole();

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DbConn"));
dataSourceBuilder.MapEnum<Role>();
dataSourceBuilder.MapEnum<Status>();
dataSourceBuilder.MapEnum<PaymentMethod>();
var dataSource = dataSourceBuilder.Build();

// adding db context into your ap
builder.Services.AddDbContext<AppDbContext>
(
    options =>
    options.UseNpgsql(dataSource)
    .UseSnakeCaseNamingConvention()
);
//Add authorization for Swagger
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Bearer token authentication",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        }
        );

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

// service registration -> automatically create all instances of dependencies
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductImageRepo, ProductImageRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ExceptionHandlerMiddleware>();

// register authorisation handler
builder.Services.AddSingleton<IAuthorizationHandler, VerifyResourceOwnerHandler>();

// add DI custom middleware
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
// add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JwtKey"])),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Secrets:Issuer"]
        };
    }
);
// add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ResourceOwner", policy =>
    {
        policy.Requirements.Add(new VerifyResourceOwnerRequirement());
    });
});
// add automapper dependency injection
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // "/swagger/index.html"
});

app.UseCors(options => options.AllowAnyOrigin());
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();

