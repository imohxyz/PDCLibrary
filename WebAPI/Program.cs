using Cinema9.Infrastructure;
using Cinema9.Logics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddLogics();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<CalculatorService>(client => 
{
    client.BaseAddress = new Uri("http://www.dneonline.com/");
    client.DefaultRequestHeaders.Add("SOAPAction", "http://tempuri.org/Multiply");
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();
await app.InitializeDatabase();

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
