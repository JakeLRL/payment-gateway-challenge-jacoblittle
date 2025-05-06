using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Database;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Requests.External;
using PaymentGateway.Api.Repositories;

namespace PaymentGateway.Api.Services;
public class PaymentsService(IPaymentsRepository paymentsRepository, IBankSimulatorService bankSimulatorService) : IPaymentsService
{
    public async Task<Payment> AddPayment(PostPaymentRequest paymentRequest)
    {
        var id = Guid.NewGuid();
        var status = PaymentStatus.Declined;

        var bankResponse = await bankSimulatorService.PostAuthorisePayment(
            new PostAuthorisePaymentRequest(paymentRequest));

        if (bankResponse?.Authorized == true)
        {
            status = PaymentStatus.Authorized;
        }

        var payment = new Payment
        {
            Id = id,
            Status = status,
            CardNumberLastFour = int.Parse(paymentRequest.CardNumber[^4..]),
            ExpiryMonth = paymentRequest.ExpiryMonth,
            ExpiryYear = paymentRequest.ExpiryYear,
            Currency = paymentRequest.Currency,
            Amount = paymentRequest.Amount
        };

        paymentsRepository.Add(payment);
        return payment;
    }

    public Payment? GetPayment(Guid id)
    {
        return paymentsRepository.Get(id);
    }
}
