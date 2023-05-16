using Microsoft.EntityFrameworkCore;
using EventusaBackend.Models;
using EventusaBackend.Models.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var databaseName = "Server=tcp:eventusa-backend-test-server.database.windows.net,1433;Initial Catalog=eventusa-backend-test-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;User ID=eventusa-backend-test-admin;Password=pAss!#$%;";

builder.Services.AddDbContext<EventContext>(options =>
options.UseSqlServer( databaseName, sqlServerOptions =>
{
    sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
})
);

builder.Services.AddDbContext<UserContext>(options =>
options.UseSqlServer(databaseName, sqlServerOptions =>
{
    sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
})
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Eventusa - Test",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
