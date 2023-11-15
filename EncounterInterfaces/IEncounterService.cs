using EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EncounterInterfaces
{
    public interface IEncounterService
    {
        
        Task<EncounterResult> GenerateEncounter(int partySize, int playerLevel, string difficulty, HttpClient _httpClient);

        Task<List<Monster>> GetMonsterList(HttpClient _httpClient);

    }
}
