using FluentValidation.TestHelper;

using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Validation;

namespace PaymentGateway.Api.Tests.Validation
{
    [TestFixture]
    public class PostPaymentRequestValidatorTests
    {
        private PostPaymentRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new PostPaymentRequestValidator();
        }

        [Test]
        public void Should_Have_No_Errors_When_Request_Is_Valid()
        {
            // Arrange
            var request = GetValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("123")]
        [TestCase("12345678901234567890")]
        [TestCase("1234abcd5678")]
        public void Should_Have_Error_For_Invalid_CardNumber(string cardNumber)
        {
            // Arrange
            var request = GetValidRequest();
            request.CardNumber = cardNumber;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CardNumber);
        }

        [TestCase(0)]
        [TestCase(13)]
        public void Should_Have_Error_For_Invalid_ExpiryMonth(int month)
        {
            // Arrange
            var request = GetValidRequest();
            request.ExpiryMonth = month;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExpiryMonth);
        }

        [Test]
        public void Should_Have_Error_If_ExpiryDate_Is_Not_In_Future()
        {
            // Arrange
            var pastMonth = DateTime.Now.Month;
            var pastYear = DateTime.Now.Year - 1;

            var request = GetValidRequest();
            request.ExpiryMonth = pastMonth;
            request.ExpiryYear = pastYear;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => new DateTime(x.ExpiryYear, x.ExpiryMonth, 1));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("AUD")]
        [TestCase("usd")] // case-sensitive
        public void Should_Have_Error_For_Invalid_Currency(string currency)
        {
            // Arrange
            var request = GetValidRequest();
            request.Currency = currency;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency);
        }

        [TestCase(0)]
        [TestCase(99)]
        [TestCase(12345)]
        public void Should_Have_Error_For_Invalid_Cvv(int cvv)
        {
            // Arrange
            var request = GetValidRequest();
            request.Cvv = cvv;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Cvv);
        }

        private static PostPaymentRequest GetValidRequest()
        {
            return new PostPaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpiryMonth = 12,
                ExpiryYear = DateTime.Now.Year + 1,
                Currency = "EUR",
                Amount = 100,
                Cvv = 123
            };
        }
    }
}
