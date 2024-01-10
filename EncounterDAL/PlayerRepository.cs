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
        public List<EncounterResult> GetEncountersByPlayerId(int id)
        {
            List<EncounterResult> encounters = new List<EncounterResult>();

            using (MySqlConnection con = new MySqlConnection(_dbConString))
            {
                con.Open();

                string q = "SELECT * FROM encounter WHERE PlayerId = @id";
                using (MySqlCommand comm = new MySqlCommand(q, con))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    
                    using (MySqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EncounterResult encounter = new EncounterResult
                            {
                               Id = Convert.ToInt32(reader["Id"]),
                               Difficulty = reader["Difficulty"].ToString(),
                               AdjustedExp = Convert.ToInt32(reader["ExpReward"]),
                               
                            };

                            encounters.Add(encounter);
                        }
                    }
                }
            }

            return encounters;
        }

    }
    
}