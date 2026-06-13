namespace Domain.Entities;

public class UserRole
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}