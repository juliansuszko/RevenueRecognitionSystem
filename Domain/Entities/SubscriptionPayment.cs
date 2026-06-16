namespace Domain.Entities;

public class SubscriptionPayment
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int ClientId { get; set; }

    public int SubscriptionId { get; set; }
    
    public virtual Client Client { get; set; } = null!;
    public virtual Subscription Subscription { get; set; } = null!;
}