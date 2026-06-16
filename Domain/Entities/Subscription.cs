namespace Domain.Entities;

public class Subscription
{
    public int SubscriptionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PeriodInMonths { get; set; }
    public decimal Price { get; set; }
    public DateTime ActiveUntil { get; set; }
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    
    public virtual Client Client { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual ICollection<SubscriptionPayment> SubscriptionPayments { get; set; } = new List<SubscriptionPayment>();
}