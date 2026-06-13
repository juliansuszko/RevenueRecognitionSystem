namespace Domain.Entities;

public class UserRole
{
    public int RoleId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}