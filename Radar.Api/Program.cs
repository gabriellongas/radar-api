using Radar.Api.Data;
using Radar.Api.Data.Context;
using Radar.Api.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RADARContext, RADARContext>(_ =>
{
    return new RADARContext(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
