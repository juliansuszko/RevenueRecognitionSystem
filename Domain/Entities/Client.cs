namespace Domain.Entities;

public class Client
{
    public int ClientId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();
    public virtual ICollection<SubscriptionPayment> SubscriptionPayments { get; set; } = new List<SubscriptionPayment>();
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}