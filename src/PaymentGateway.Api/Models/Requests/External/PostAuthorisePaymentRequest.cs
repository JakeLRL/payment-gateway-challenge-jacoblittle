using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Requests.External;
public class PostAuthorisePaymentRequest
{
    [JsonPropertyName("card_number")]
    public string CardNumber { get; set; }
    [JsonPropertyName("expiry_date")]
    public string ExpiryDate { get; set; }
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("cvv")]
    public string Cvv { get; set; }

    public PostAuthorisePaymentRequest(PostPaymentRequest paymentRequest)
    {
        CardNumber = paymentRequest.CardNumber;
        ExpiryDate = $"{paymentRequest.ExpiryMonth:D2}/{paymentRequest.ExpiryYear}";
        Currency = paymentRequest.Currency;
        Amount = paymentRequest.Amount;
        Cvv = paymentRequest.Cvv.ToString();
    }
}
