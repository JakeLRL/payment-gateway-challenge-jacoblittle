using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models.Database;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Requests.External;
using PaymentGateway.Api.Models.Responses.External;
using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests.Controllers;

[TestFixture]
public class PaymentControllerTests
{
    private readonly Random _random = new();

    private PaymentsRepository _paymentsRepository;
    private PaymentsService _paymentsService;
    private IBankSimulatorService _bankSimulatorService;

    private PaymentsController _sut;

    [SetUp]
    public void SetUp()
    {
        _paymentsRepository = new PaymentsRepository();
        _bankSimulatorService = Substitute.For<IBankSimulatorService>();
        _paymentsService = new PaymentsService(_paymentsRepository, _bankSimulatorService);

        _sut = new PaymentsController(_paymentsService);
    }

    [Test]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ExpiryYear = _random.Next(2023, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumberLastFour = _random.Next(1111, 9999),
            Currency = "GBP"
        };
        _paymentsRepository.Add(payment);

        // Act
        var response = await _sut.GetPaymentAsync(payment.Id);

        // Assert
        var result = response.Result as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.Not.Null);
    }

    [Test]
    public async Task Returns404IfPaymentNotFound()
    {
        // Act
        var response = await _sut.GetPaymentAsync(Guid.NewGuid());

        // Assert
        var result = response.Result as NotFoundResult;
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task PostsPaymentSuccessfully()
    {
        // Arrange
        var payment = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(2026, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumber = "2222405343248877",
            Currency = "GBP",
            Cvv = _random.Next(100, 9999)
        };
        var authorisePaymentResponse = new PostAuthorisePaymentResponse
        {
            Authorized = true,
            AuthorizationCode = Guid.NewGuid().ToString()
        };

        _bankSimulatorService.PostAuthorisePayment(Arg.Any<PostAuthorisePaymentRequest>())
            .Returns(authorisePaymentResponse);

        // Act
        var response = await _sut.PostPaymentAsync(payment);

        // Assert
        var result = response.Result as OkObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.Not.Null);
    }
}