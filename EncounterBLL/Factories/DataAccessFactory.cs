using EncounterInterfaces;
using EncounterDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EncounterBLL.Factories
{
    public class DataAccessFactory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public DataAccessFactory(IMemoryCache memoryCache, IConfiguration configuration){
            
            _memoryCache = memoryCache;
            _configuration = configuration;
          
           
        }

        public IMonsterApiService GetAPI()
        {
            MonsterApi monsterApi = new MonsterApi(_memoryCache);
            return monsterApi;
        }

        public IEncounterRepository GetEncounterRepository(){
            string conString = _configuration.GetConnectionString("Database");
            EncounterRespository encounterRespository = new(conString);
            return encounterRespository;
        }

        public IPlayerRepository GetPlayerRepository(){
            string conString = _configuration.GetConnectionString("Database");
            PlayerRepository playerRepository = new(conString);
            return playerRepository;
        }
    }
}
