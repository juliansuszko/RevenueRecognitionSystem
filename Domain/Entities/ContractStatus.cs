namespace Domain.Entities;

public class ContractStatus
{
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    
}