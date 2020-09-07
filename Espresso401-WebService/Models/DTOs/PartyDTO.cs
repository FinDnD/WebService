using System.Collections.Generic;

namespace Espresso401_WebService.Models.DTOs
{
    public class PartyDTO
    {
        public int Id { get; set; }
        public int DungeonMasterId { get; set; }
        public int MaxSize { get; set; }
        public bool Full { get; set; }
        public PartyDmDTO DungeonMasterDTO { get; set; }
        public List<PartyPlayerDTO> PlayersInParty { get; set; }
    }
}