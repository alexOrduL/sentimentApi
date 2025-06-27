using Microsoft.EntityFrameworkCore;
using SentimentApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger sin caché
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware para Swagger (con parámetros que evitan caché)
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true; // Opcional: para compatibilidad
});

app.UseSwaggerUI(options =>
{
    options.ConfigObject.DisplayRequestDuration = true; // Ejemplo de otra configuración
    // Desactiva caché mediante versión única en URL
    options.SwaggerEndpoint("/swagger/v1/swagger.json?t=" + DateTime.Now.Ticks, "v1");
});

// Migración de base de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();
app.Run();