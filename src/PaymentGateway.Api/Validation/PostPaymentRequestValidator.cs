using FluentValidation;

using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Validation;
public class PostPaymentRequestValidator : AbstractValidator<PostPaymentRequest>
{
    private static readonly HashSet<string> AllowedCurrencies = ["GBP", "USD", "EUR"];
    public PostPaymentRequestValidator()
    {
        RuleFor(paymentRequest => paymentRequest.CardNumber)
            .NotEmpty().WithMessage("CardNumber is a required field.")
            .Length(14, 19).WithMessage("CardNumber must be 14-19 digits long.")
            .Matches(@"^\d+$").WithMessage("The value must contain only digits.");

        RuleFor(paymentRequest => paymentRequest.ExpiryMonth)
            .NotNull().WithMessage("ExpiryMonth is a required field.")
            .InclusiveBetween(1, 12).WithMessage("ExpiryMonth must be between 1-12.");

        RuleFor(paymentRequest => paymentRequest.ExpiryYear)
            .NotNull().WithMessage("ExpiryYear is a required field.")
            .GreaterThan(0);

        RuleFor(x => x)
            .Must(x =>
            {
                if (x.ExpiryMonth < 1 || x.ExpiryMonth > 12 || x.ExpiryYear <= 0)
                    return true;

                var expiryDate = new DateTime(x.ExpiryYear, x.ExpiryMonth, 1);
                var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                return expiryDate > currentMonthStart;
            })
            .WithMessage("The Expiry date must be in the future.");

        RuleFor(paymentRequest => paymentRequest.Currency)
            .NotEmpty().WithMessage("Currency is a required field.")
            .Must(AllowedCurrencies.Contains)
            .WithMessage("Currency must be either GBP, USD, or EUR.");

        // already takes an int "Must be an integer"
        RuleFor(paymentRequest => paymentRequest.Amount)
            .NotNull().WithMessage("Amount is a required field.");

        RuleFor(paymentRequest => paymentRequest.Cvv)
            .NotEmpty().WithMessage("Cvv is a required field.")
            .InclusiveBetween(100, 9999);

    }
}
