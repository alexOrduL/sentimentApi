using Microsoft.EntityFrameworkCore;
using SentimentApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Forzar escucha en todas las IPs del contenedor
builder.WebHost.UseUrls("http://0.0.0.0:80");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SentimentAnalyzer>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

Console.WriteLine("Connection string:");
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

// Usar Swagger solo en desarrollo
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
