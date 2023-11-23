using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterModels;
using EncounterInterfaces;
using MySql.Data.MySqlClient;

namespace EncounterDAL
{
    public class EncounterRespository : IEncounterRepository
    {

        private readonly string conString = "Server=localhost;User ID=root;Password='';Database=encounter_creator";

        public async Task<bool> SaveEncounter(EncounterResult encounterResult)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    await con.OpenAsync();

                    using (MySqlTransaction transaction = await con.BeginTransactionAsync())
                    {
                        try
                        {
                        
                            string encounterQuery = "INSERT INTO encounter (Name, Difficulty, ExpReward, PlayerId) " +
                                                    "VALUES (@name, @difficulty, @expReward, @playerId)";

                            long lastInsertedEncounterId;
                            using (MySqlCommand encounterCommand = new MySqlCommand(encounterQuery, con))
                            {
                                encounterCommand.Transaction = transaction;

                                encounterCommand.Parameters.AddWithValue("@name", "Placeholder Name");
                                encounterCommand.Parameters.AddWithValue("@difficulty", encounterResult.Difficulty);
                                encounterCommand.Parameters.AddWithValue("@expReward", encounterResult.AdjustedExp);
                                encounterCommand.Parameters.AddWithValue("@playerId", 1);

                                await encounterCommand.ExecuteNonQueryAsync();

                                lastInsertedEncounterId = encounterCommand.LastInsertedId;

                            }

                            
                            foreach (var monster in encounterResult.Monsters)
                            {
                                string monsterQuery = "INSERT INTO monster (Name, Size, Type, CR, Exp)" +
                                                    "VALUES (@name, @size, @type, @cr, @exp)";

                                using (MySqlCommand monsterCommand = new MySqlCommand(monsterQuery, con))
                                {
                                    monsterCommand.Transaction = transaction;

                                    monsterCommand.Parameters.AddWithValue("@name", monster.Name);
                                    monsterCommand.Parameters.AddWithValue("@size", monster.Size);
                                    monsterCommand.Parameters.AddWithValue("@type", monster.Type);
                                    monsterCommand.Parameters.AddWithValue("@cr", monster.CR);
                                    monsterCommand.Parameters.AddWithValue("@exp", monster.ExperiencePoints);

                                    await monsterCommand.ExecuteNonQueryAsync();

                                    long lastInsertedMonsterId = monsterCommand.LastInsertedId;

                                    string encounterMonsterQuery = "INSERT INTO encounter_monster (EncounterId, MonsterId) " +
                                                "VALUES (@EncounterId, @monsterId)";
                                    using (MySqlCommand encounterMonsterCommand = new MySqlCommand(encounterMonsterQuery, con))
                                    {
                                        encounterMonsterCommand.Transaction = transaction;

                                        encounterMonsterCommand.Parameters.AddWithValue("@EncounterId", lastInsertedEncounterId);
                                        encounterMonsterCommand.Parameters.AddWithValue("@monsterId", lastInsertedMonsterId);

                                        await encounterMonsterCommand.ExecuteNonQueryAsync();
                                    }

                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine($"EncounterRepository: An error occurred while saving the encounter. Exception message: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the encounter. Exception message: {ex.Message}");
                return false;
            }
        }

    }
}
