using NSubstitute;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Database;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Requests.External;
using PaymentGateway.Api.Models.Responses.External;
using PaymentGateway.Api.Repositories;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests.Services
{
    [TestFixture]
    public class PaymentsServiceTests
    {
        private IPaymentsRepository _paymentsRepository;
        private IBankSimulatorService _bankSimulatorService;
        private PaymentsService _paymentsService;

        [SetUp]
        public void SetUp()
        {
            _paymentsRepository = Substitute.For<IPaymentsRepository>();
            _bankSimulatorService = Substitute.For<IBankSimulatorService>();
            _paymentsService = new PaymentsService(_paymentsRepository, _bankSimulatorService);
        }

        [Test]
        public async Task AddPayment_ShouldReturnAuthorizedPayment_WhenBankAuthorizes()
        {
            // Arrange
            var request = new PostPaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                Currency = "USD",
                Amount = 100
            };

            _bankSimulatorService.PostAuthorisePayment(Arg.Any<PostAuthorisePaymentRequest>())
                .Returns(new PostAuthorisePaymentResponse { Authorized = true });

            // Act
            var result = await _paymentsService.AddPayment(request);

            // Assert
            Assert.That(PaymentStatus.Authorized, Is.EqualTo(result.Status));
            Assert.That(1111, Is.EqualTo(result.CardNumberLastFour));
            _paymentsRepository.Received(1).Add(Arg.Any<Payment>());
        }

        [Test]
        public async Task AddPayment_ShouldReturnDeclinedPayment_WhenBankDeclines()
        {
            // Arrange
            var request = new PostPaymentRequest
            {
                CardNumber = "5555555555554444",
                ExpiryMonth = 6,
                ExpiryYear = 2026,
                Currency = "EUR",
                Amount = 200
            };

            _bankSimulatorService.PostAuthorisePayment(Arg.Any<PostAuthorisePaymentRequest>())
                .Returns(new PostAuthorisePaymentResponse { Authorized = false });

            // Act
            var result = await _paymentsService.AddPayment(request);

            // Assert
            Assert.That(PaymentStatus.Declined, Is.EqualTo(result.Status));
            Assert.That(4444, Is.EqualTo(result.CardNumberLastFour));
            _paymentsRepository.Received(1).Add(Arg.Any<Payment>());
        }

        [Test]
        public void GetPayment_ShouldReturnPaymentFromRepository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var payment = new Payment { Id = id, Amount = 100 };
            _paymentsRepository.Get(id).Returns(payment);

            // Act
            var result = _paymentsService.GetPayment(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(id, Is.EqualTo(result?.Id));
            Assert.That(100, Is.EqualTo(result?.Amount));
        }

        [Test]
        public void GetPayment_ShouldReturnNull_WhenPaymentNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _paymentsRepository.Get(id).Returns((Payment)null);

            // Act
            var result = _paymentsService.GetPayment(id);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
