using PaymentGateway.Api.Models.Database;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Services;
public interface IPaymentsService
{
    Task<Payment?> AddPayment(PostPaymentRequest paymentRequest);
    Payment? GetPayment(Guid id);
}
