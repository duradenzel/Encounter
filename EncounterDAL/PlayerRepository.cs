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

    public List<Player> GetAllPlayers()
    {
        List<Player> players = new List<Player>();

        using (MySqlConnection con = new MySqlConnection(_dbConString))
        {
            con.Open();

            string q = "SELECT * FROM player";
            using (MySqlCommand comm = new MySqlCommand(q, con))
            {
                
                using (MySqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Player player = new Player
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString()
                        };

                        players.Add(player);
                    }
                }
            }
        }

        return players;
    }

    }
    
}