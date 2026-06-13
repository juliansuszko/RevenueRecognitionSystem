namespace Domain.Entities;

public class SoftwareCategory
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Software> Softwares { get; set; } = new List<Software>();
}