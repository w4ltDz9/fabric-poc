using Fabric.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<ContractEvaluationService>();

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // 👈 Required for Swagger
builder.Services.AddSwaggerGen();           // 👈 Registers Swagger

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                       // 👈 Serves the Swagger JSON
    app.UseSwaggerUI();                     // 👈 Serves the Swagger GUI
}

app.MapControllers();

app.Run();