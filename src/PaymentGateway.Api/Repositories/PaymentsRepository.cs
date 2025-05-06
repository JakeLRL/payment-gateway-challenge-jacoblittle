using PaymentGateway.Api.Models.Database;

namespace PaymentGateway.Api.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    public List<Payment> Payments = [];
    
    public void Add(Payment payment)
    {
        // if using EF then when adding the object it would assign the Id
        // but we will assign manually in the service layer
        Payments.Add(payment);
    }

    public Payment? Get(Guid id)
    {
        return Payments.FirstOrDefault(p => p.Id == id);
    }
}