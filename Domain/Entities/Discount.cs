namespace Domain.Entities;

public class Discount
{
    public int DiscountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int SoftwareId { get; set; }

    public virtual Software Software { get; set; } = null!;
}