using System.Configuration;
using System.Configuration.Internal;
using EncounterDAL;
using EncounterInterfaces;
using EncounterModels;
using Microsoft.Extensions.Caching.Memory;

namespace Testing;

[TestClass]
public class DatabaseTests
{
    private readonly IEncounterRepository _encounterRepository;
    private readonly string conString = "Server=localhost;User ID=root;Password='';Database=encounter_creator";


    public DatabaseTests()
    {
        _encounterRepository = new EncounterRespository(conString);
    }

    [TestMethod]
    public async Task SaveEncounter_SaveEncounterSuccessful()
    {
        
        var encounterResult = new EncounterResult
        {
            Difficulty = "Medium",
            AdjustedExp = 1000,
            Monsters = new List<Monster>
            {
                new Monster
                {
                    Name = "Goblin",
                    Size = "Small",
                    Type = "Humanoid",
                    CR = 1,
                    ExperiencePoints = 100
                }
            }
        };

        
        var result = await _encounterRepository.SaveEncounter(encounterResult);

        
        Assert.IsTrue(result, "Saving encounter should succeed");
    }

    [TestMethod]
    public async Task SaveEncounter_ShouldRollbackTransactionOnException()
    {
        var encounterResult = new EncounterResult
        {
            Difficulty = "Easy",
            AdjustedExp = 500,
            Monsters = new List<Monster>
            {
                new Monster
                {
                    Name = "Skeleton",
                    Size = "Medium",
                    Type = "Undead",
                    CR = 1,
                    ExperiencePoints = 200
                },
                new Monster
                {
                }
            }
        };

        
        var result = await _encounterRepository.SaveEncounter(encounterResult);

        
        Assert.IsFalse(result, "Saving encounter should fail due to invalid monster data");
    }

}