using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.DAO.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================
// DEPENDENCY INJECTION
// =======================
builder.Services.AddScoped<ICategoriaDAO, CategoriaDAOImpl>();
builder.Services.AddScoped<IProductoDAO, ProductoDAOImpl>();
builder.Services.AddScoped<IRolDAO, RolDAOImpl>();
builder.Services.AddScoped<ITamanioDAO, TamanioDAOImpl>();
builder.Services.AddScoped<IUsuarioDAO, UsuarioDAOImpl>();
builder.Services.AddScoped<IPedidoDAO, PedidoDAOImpl>();
builder.Services.AddScoped<IOpcionGrupoDAO, OpcionGrupoDAOImpl>();
builder.Services.AddScoped<IOpcionDAO, OpcionDAOImpl>();
builder.Services.AddScoped<IProductoOpcionDAO, ProductoOpcionDAOImpl>();
builder.Services.AddScoped<IMetodoPagoDAO, MetodoPagoDAOImpl>();

builder.Services.AddControllers();

// =======================
// SWAGGER + JWT
// =======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cafeteria API",
        Version = "v1"
    });

    //  DEFINICIÓN JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese: Bearer {token}"
    });

    // USO JWT
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

// =======================
// AUTHENTICATION JWT
// =======================

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),

        ClockSkew = TimeSpan.Zero //
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// =======================
// MIDDLEWARE PIPELINE
// =======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization(); 

app.MapControllers();

app.Run();
