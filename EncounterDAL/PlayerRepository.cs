using EncounterModels;
using EncounterInterfaces;
using MySql.Data.MySqlClient;

namespace EncounterDAL
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly string _dbConString;
        public PlayerRepository(string dbConString){
            _dbConString = dbConString;
        }
    }
    
}