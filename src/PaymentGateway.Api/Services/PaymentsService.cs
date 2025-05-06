using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Database;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Requests.External;
using PaymentGateway.Api.Repositories;

namespace PaymentGateway.Api.Services;
public class PaymentsService(IPaymentsRepository paymentsRepository,
    IBankSimulatorService bankSimulatorService,
    ILogger<PaymentsService> logger) : IPaymentsService
{
    public async Task<Payment?> AddPayment(PostPaymentRequest paymentRequest)
    {
        var paymentId = Guid.NewGuid();
        var status = PaymentStatus.Declined;

        var bankRequest = new PostAuthorisePaymentRequest(paymentRequest);
        var bankResponse = await bankSimulatorService.PostAuthorisePayment(bankRequest);

        if (bankResponse is not null)
        {
            logger.LogInformation(
                "Authorise payment with code: {AuthorizationCode} returned Authorized = {Authorized}",
                bankResponse.AuthorizationCode, bankResponse.Authorized);

            if (bankResponse.Authorized)
            {
                status = PaymentStatus.Authorized;
            }
        }
        else
        {
            logger.LogWarning("No response receive from BankSimulator");
            return null;
        }

        var payment = new Payment(paymentRequest)
        {
            Id = paymentId,
            Status = status
        };

        paymentsRepository.Add(payment);
        logger.LogInformation("Payments added with Id: {Id}", payment.Id);

        return payment;
    }

    public Payment? GetPayment(Guid id)
    {
        return paymentsRepository.Get(id);
    }
}
