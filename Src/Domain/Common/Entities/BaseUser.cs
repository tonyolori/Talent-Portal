﻿using Domain.Enum;
using Microsoft.AspNetCore.Identity;
namespace Domain.Common.Entities;
public abstract class BaseUser : IdentityUser
{
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    
    public UserRoles Role { get; set; }

    public string RoleDesc { get; set; }
    
    
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    
    public string? LastModifiedBy { get; set; } = string.Empty;
    
    public DateTime? LastModifiedDate { get; set; }
}
