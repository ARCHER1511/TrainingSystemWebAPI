using Microsoft.AspNetCore.Identity;

namespace TrainingSystemAPI.Models
{
    public class AppUser : IdentityUser
    {
       public string Address { get; set; } = string.Empty;
    }
}
