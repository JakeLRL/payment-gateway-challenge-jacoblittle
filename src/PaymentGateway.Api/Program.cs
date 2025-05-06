using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders().AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("BankSimulatorClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddSingleton<IBankSimulatorService, BankSimulatorService>();

builder.Services.AddSingleton<IPaymentsService, PaymentsService>();
builder.Services.AddSingleton<IPaymentsRepository, PaymentsRepository>();

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
