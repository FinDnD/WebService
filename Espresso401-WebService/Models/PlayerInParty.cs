namespace Espresso401_WebService.Models
{
    public class PlayerInParty
    {
        public int PartyId { get; set; }
        public int? PlayerId { get; set; }

        public Player Player { get; set; }
        public Party Party { get; set; }
    }
}