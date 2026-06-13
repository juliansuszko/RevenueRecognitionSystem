namespace Domain.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int ClientId { get; set; }

    public int? ContractId { get; set; }
    public int? SubscriptionId { get; set; }
    
    public virtual Client Client { get; set; } = null!;
    public virtual Contract? Contract { get; set; }
    public virtual Subscription? Subscription { get; set; }
}