using Domain.Enum;
using Microsoft.AspNetCore.Identity;
namespace Domain.Common.Entities;
public abstract class BaseUser : IdentityUser
{
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth;
    
    public UserRoles Role { get; set; }

    public string RoleDesc { get; set; }
    
    public Status UserStatus { get; set; }
        
    public string UserStatusDes { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public string LastModifiedBy { get; set; } = string.Empty;
    
    public DateTime LastModifiedDate { get; set; }
}
