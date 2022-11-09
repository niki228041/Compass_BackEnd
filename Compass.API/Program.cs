using Compass.API.Infrasructure.AutoMapper;
using Compass.API.Infrasructure.Repositories;
using Compass.API.Infrasructure.Services;
using Compass.Data.Data.AutoMapper;
using Compass.Data.Data.Classes;
using Compass.Data.Data.Context;
using Compass.Data.Data.Interfaces;
using Compass.Data.Data.Models;
using Compass.Data.Initializer;
using Compass.Services;
using Compass.Services.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
builder.Services.AddSwaggerGen((options) =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add dababase context
builder.Services.AddDbContext<AppDbContext>();



    //Add AutoMapper Configuration
AutoMapperConfiguration.Configuration(builder.Services);

    //Add Services Configuration
ServicesConfiguration.Configuration(builder.Services,builder.Configuration);

    //Add Repositories Configuration
RepositoriesConfiguration.Configuration(builder.Services);




// Add Identity user and roles
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add JWT Service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

// Add RazorPages
builder.Services.AddRazorPages();


builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]);
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = false,
    RequireExpirationTime = false,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt => {
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .SetIsOriginAllowed(origin => true)
    //.WithOrigins(new[] { "http://localhost:3000" })
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    );

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();




await AppDbInitializer.Seed(app);
app.Run();


