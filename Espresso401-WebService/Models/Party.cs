using System.Collections.Generic;

namespace Espresso401_WebService.Models
{
    public class Party
    {
        public int Id { get; set; }
        public int DungeonMasterId { get; set; }
        public int MaxSize { get; set; }
        public bool Full { get; set; }
        public DungeonMaster DungeonMaster { get; set; }
        public List<PlayerInParty> PlayersInParty { get; set; }
    }
}