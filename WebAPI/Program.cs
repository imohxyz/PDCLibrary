using Cinema9.Infrastructure;
using Cinema9.Logics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddLogics();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var pathBase = builder.Configuration["PathBase"];

if (!string.IsNullOrWhiteSpace(pathBase))
{
    app.UsePathBase(pathBase);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
