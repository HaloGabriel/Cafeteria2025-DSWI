using Cafeteria2025_API_REST.Configs;
using Cafeteria2025_API_REST.DAO;
using Cafeteria2025_API_REST.DAO.Impl;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICategoriaDAO, CategoriaDAOImpl>();
builder.Services.AddScoped<IProductoDAO, ProductoDAOImpl>();
builder.Services.AddScoped<IRolDAO, RolDAOImpl>();
builder.Services.AddScoped<ITamanioDAO, TamanioDAOImpl>();
builder.Services.AddScoped<IUsuarioDAO, UsuarioDAOImpl>();
builder.Services.AddScoped<IPedidoDAO, PedidoDAOImpl>();
builder.Services.AddScoped<IOpcionGrupoDAO, OpcionGrupoDAOImpl>();
builder.Services.AddScoped<IOpcionDAO, OpcionDAOImpl>();
builder.Services.AddScoped<IProductoOpcionDAO, ProductoOpcionDAOImpl>();



// Add configurations to the container
MapsterConfig.RegisterMappings();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
