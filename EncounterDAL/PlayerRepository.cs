using EncounterModels;
using EncounterInterfaces;
using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

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
                                PlayerLevels = JsonConvert.DeserializeObject<int[]>(reader["PlayerLevels"].ToString()),

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

         public EncounterResult GetEncounter(int id)
        {
        EncounterResult encounter = new EncounterResult();
        using (MySqlConnection con = new MySqlConnection(_dbConString))
        {
            con.Open();

            // Fetch encounter details
            string encounterQuery = "SELECT * FROM encounter WHERE Id = @id";
            using (MySqlCommand encounterComm = new MySqlCommand(encounterQuery, con))
            {
                encounterComm.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader encounterReader = encounterComm.ExecuteReader())
                {
                    while (encounterReader.Read())
                    {
                        encounter = new EncounterResult
                        {
                            Id = Convert.ToInt32(encounterReader["Id"]),
                            PlayerLevels = JsonConvert.DeserializeObject<int[]>(encounterReader["PlayerLevels"].ToString()),
                            Difficulty = encounterReader["Difficulty"].ToString(),
                            AdjustedExp = Convert.ToInt32(encounterReader["ExpReward"]),
                            Monsters = new List<Monster>() 
                        };
                    }
                }
            }

            // Fetch associated monsters
            string monsterQuery = "SELECT m.* FROM monster m " +
                                    "JOIN encounter_monster em ON m.Id = em.MonsterId " +
                                    "WHERE em.EncounterId = @id";

            using (MySqlCommand monsterComm = new MySqlCommand(monsterQuery, con))
            {
                monsterComm.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader monsterReader = monsterComm.ExecuteReader())
                {
                    while (monsterReader.Read())
                    {
                        Monster monster = new Monster
                        {
                            
                            Name = monsterReader["Name"].ToString(),
                            CR = Convert.ToDouble(monsterReader["CR"]),
                            Size = monsterReader["Size"].ToString()
                        };

                        encounter.Monsters.Add(monster);
                    }
                }
            }
        }

        return encounter;
        }



    }
    
}