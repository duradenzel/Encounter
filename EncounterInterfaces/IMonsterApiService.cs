using EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncounterInterfaces
{
    public interface IMonsterApiService
    {
        Task<List<Monster>> GetMonsters(int desiredXpValue, HttpClient _httpClient);
        Task<List<Monster>> GetMonsterList(HttpClient _httpClient);

    }
}

