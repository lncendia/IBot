using IBot.Core.Entities.Users;

namespace IBot.DAL.Models;

public class UserProductModel
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = null!;
    
    public Guid ProductId { get; set; }
}