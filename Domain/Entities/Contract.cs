namespace Domain.Entities;

public class Contract
{
    public int ContractId { get; set; }
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public string SoftwareVersion { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int YearsOfSupport { get; set; }
    public decimal Price { get; set; }
    public int StatusId { get; set; }
    
    public virtual Client Client { get; set; } = null!;
    public virtual Software Software { get; set; } = null!;
    public virtual ContractStatus Status { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}