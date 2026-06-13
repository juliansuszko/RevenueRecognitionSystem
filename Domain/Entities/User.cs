namespace Domain.Entities;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    
    public virtual UserRole UserRole { get; set; } = null!;
    public virtual Token Token { get; set; } = null!;
}