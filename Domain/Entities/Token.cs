namespace Domain.Entities;

public class Token
{
    public int TokenId { get; set; }
    public string RefreshTokenValue { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
}