using Microsoft.AspNetCore.Identity;

namespace ElectronicsStore.API.Models
{
    // Extending the default IdentityUser to include custom properties
    public class ApplicationUser : IdentityUser
    {
        // Full name of the user for display purposes
        public string? FullName { get; set; }
    }
}