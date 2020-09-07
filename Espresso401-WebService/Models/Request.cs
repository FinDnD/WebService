namespace Espresso401_WebService.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int DungeonMasterId { get; set; }
        public bool PlayerAccepted { get; set; }
        public bool DungeonMasterAccepted { get; set; }
        public bool Active { get; set; }
        public DungeonMaster DungeonMaster { get; set; }
        public Player Player { get; set; }
    }
}