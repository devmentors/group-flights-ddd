using FakePaymentGateway;
using FakePaymentGateway.DTO;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<FakePaymentProcessor>();
builder.Services.AddScoped<FakePaymentProcessor>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapPost("/payment-gateway",
    ([FromServices] FakePaymentProcessor processor,
        [FromBody] SetUpPaymentWithMetadata paymentSetup) =>
    {
        processor.SetUpPayment(paymentSetup);
    });

app.Run();