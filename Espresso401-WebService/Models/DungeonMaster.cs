using System.Collections.Generic;
using static Espresso401_WebService.Models.Enums;

namespace Espresso401_WebService.Models
{
    public class DungeonMaster
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDesc { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public string PersonalBio { get; set; }
        public string ImageUrl { get; set; }
        public Party Party { get; set; }
        public List<Request> ActiveRequests { get; set; }
    }
}