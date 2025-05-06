using PaymentGateway.Api.Models.Requests.External;
using PaymentGateway.Api.Models.Responses.External;

namespace PaymentGateway.Api.Services;
public interface IBankSimulatorService
{
    Task<PostAuthorisePaymentResponse?> PostAuthorisePayment(PostAuthorisePaymentRequest request);
}
