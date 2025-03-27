using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Cinema9.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; init; }
    public DateTime CreatedDate { get; init; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}

public class AppRole : IdentityRole<Guid> 
{
    public string Description { get; init; }

    // Navigation properties
    public virtual ICollection<IdentityRoleClaim<Guid>> RoleClaims { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}
