using Microsoft.AspNetCore.Identity;

namespace Espresso401_WebService.Models
{
    public class ApplicationUser : IdentityUser
    {
    }

    public static class ApplicationRoles
    {
        public const string Player = "Player";
        public const string DungeonMaster = "DungeonMaster";
        public const string Admin = "Admin";
    }
}