using Microsoft.AspNetCore.Identity;
namespace Domain.Common.Entities;
public abstract class BaseUser : IdentityUser
{

    public string PhoneNumber;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth;
    public DateTime CreatedDate { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime LastModifiedDate { get; set; }
}
