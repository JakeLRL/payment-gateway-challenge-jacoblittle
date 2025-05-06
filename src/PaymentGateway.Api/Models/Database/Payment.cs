using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Models.Database;
public class Payment
{
    public Guid Id { get; set; }
    public PaymentStatus Status { get; set; }
    public int CardNumberLastFour { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string? Currency { get; set; }
    public int Amount { get; set; }

    public Payment() { }
    public Payment(PostPaymentRequest paymentRequest)
    {
        CardNumberLastFour = int.Parse(paymentRequest.CardNumber[^4..]);
        ExpiryMonth = paymentRequest.ExpiryMonth;
        ExpiryYear = paymentRequest.ExpiryYear;
        Currency = paymentRequest.Currency;
        Amount = paymentRequest.Amount;
    }
}