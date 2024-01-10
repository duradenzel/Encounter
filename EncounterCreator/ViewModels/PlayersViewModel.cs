using EncounterModels;

namespace EncounterCreator.ViewModels
{
    public class PlayerViewModel
    {
        public List<Player> Players { get; set; }
        public EncounterResult Encounter { get; set; }

        public List<EncounterResult> PlayerEncounters { get; set; }

    }
}
