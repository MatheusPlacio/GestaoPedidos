using GestaoPedidos.Application.Services;
using GestaoPedidos.Domain.Interfaces.Repositories;
using GestaoPedidos.Infra.Data;
using GestaoPedidos.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Data
builder.Services.AddSingleton<IDbConnectionFactory>(provider => 
    new SqlServerConnectionFactory(connectionString!));

// Repositories
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPromocaoRepository, PromocaoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Services
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<PromocaoService>();
builder.Services.AddScoped<PedidoService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
