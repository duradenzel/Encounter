using EncounterInterfaces;
using EncounterDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EncounterBLL.Factories
{
    public class DataAccessFactory
    {
        private readonly IMemoryCache _memoryCache;

        public DataAccessFactory(IMemoryCache memoryCache){
            _memoryCache = memoryCache;
        }

        public IMonsterApiService GetAPI()
        {
            MonsterApi monsterApi = new(_memoryCache);
            return monsterApi;
        }

        public IEncounterRepository GetEncounterRepository(){
            EncounterRespository encounterRespository = new();
            return encounterRespository;
        }
    }
}
