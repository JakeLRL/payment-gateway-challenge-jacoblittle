using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Api.Models.Requests;

public class PostPaymentRequest
{
    //changed from last 4 digits and to a string as ints don't support
    [Required]
    public string CardNumber { get; set; }
    [Required]
    public int ExpiryMonth { get; set; }
    [Required]
    public int ExpiryYear { get; set; }
    [Required]
    public string Currency { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public int Cvv { get; set; }
}