using EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncounterInterfaces
{
    public interface IPlayerRepository
    {     
        List<Player> GetAllPlayers();
        List<EncounterResult> GetEncountersByPlayerId(int id);
         EncounterResult GetEncounter(int id);


    }
}

