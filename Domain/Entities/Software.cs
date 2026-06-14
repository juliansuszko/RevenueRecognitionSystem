namespace Domain.Entities;

public class Software
{
    public int SoftwareId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LatestVersion { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    
    public decimal BasePrice { get; set; }

    public virtual SoftwareCategory Category { get; set; } = null!;
    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}