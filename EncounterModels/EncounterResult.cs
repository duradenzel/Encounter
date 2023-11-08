using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterModels
{
    public class EncounterResult
    {
        public List<Monster> Monsters { get; set; }
        public string Difficulty { get; set; }

        public int TotalExp { get; set; }
        public int AdjustedExp { get; set; }

        public Dictionary<string, int> XpSums { get; set; }



    }


}
