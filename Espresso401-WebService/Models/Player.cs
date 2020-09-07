using System.Collections.Generic;
using static Espresso401_WebService.Models.Enums;

namespace Espresso401_WebService.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public string CharacterName { get; set; }
        public Class Class { get; set; }
        public Race Race { get; set; }
        public ExperienceLevel ExperienceLevel { get; set; }
        public int GoodAlignment { get; set; }
        public int LawAlignment { get; set; }
        public int? PartyId { get; set; }
        public Party Party { get; set; }
        public List<Request> ActiveRequests { get; set; }
    }
}