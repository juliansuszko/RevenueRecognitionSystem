namespace Domain.Entities;

public class Token
{
    public Guid UserId { get; set; } 
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; } 

    public virtual User User { get; set; } = null!;
}