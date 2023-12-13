
using EncounterBLL;
using EncounterInterfaces;
using EncounterModels;
using EncounterBLL.Factories;
using MySqlX.XDevAPI;

public class EncounterService
{
    private readonly DataAccessFactory _dataAccessFactory;
    private readonly IMonsterApiService _monsterApiService;
    private readonly IHttpClientFactory _clientFactory;

    private readonly IEncounterRepository _encounterRepository;
    
    
    public EncounterService(DataAccessFactory dataAccessFactory, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _dataAccessFactory = dataAccessFactory;
        _monsterApiService = _dataAccessFactory.GetAPI();
        _encounterRepository = _dataAccessFactory.GetEncounterRepository();
    }

    public async Task<EncounterResult> GenerateEncounter(int partySize, int playerLevel, string difficulty)
    {
        int[] playerLevels = Enumerable.Repeat(playerLevel, partySize).ToArray();

        List<XpThresholds> xpThresholds = CalculatePartyXpThresholds(playerLevels, difficulty);

        int desiredXpValue = 0;
        switch (difficulty)
        {
            case "easy":
                desiredXpValue = xpThresholds.Sum(thresholds => thresholds.Easy);
                break;
            case "medium":
                desiredXpValue = xpThresholds.Sum(thresholds => thresholds.Medium);
                break;
            case "hard":
                desiredXpValue = xpThresholds.Sum(thresholds => thresholds.Hard);
                break;
            case "deadly":
                desiredXpValue = xpThresholds.Sum(thresholds => thresholds.Deadly);
                break;
            default:
                break;
        }

        Dictionary<string, int> xpSums = CalculateAllXpSums(xpThresholds);

        
       using (HttpClient _client = _clientFactory.CreateClient())
        {
            List<Monster> selectedMonsters = await _monsterApiService.GetMonsters(desiredXpValue, _client);
             int adjustedXpValue = CalculateAdjustedXpValue(selectedMonsters.Sum(monster => monster.ExperiencePoints), selectedMonsters.Count);


            EncounterResult result = new EncounterResult
            {
                Monsters = selectedMonsters,
                Difficulty = difficulty,
                TotalExp = selectedMonsters.Sum(monster => monster.ExperiencePoints),
                XpSums = xpSums,
                AdjustedExp = adjustedXpValue
            };
        return result;
        }
        

       
    }


    private Dictionary<string, int> CalculateAllXpSums(List<XpThresholds> xpThresholds)
    {
        Dictionary<string, int> xpSums = new Dictionary<string, int>
    {
        { "easy", xpThresholds.Sum(thresholds => thresholds.Easy) },
        { "medium", xpThresholds.Sum(thresholds => thresholds.Medium) },
        { "hard", xpThresholds.Sum(thresholds => thresholds.Hard) },
        { "deadly", xpThresholds.Sum(thresholds => thresholds.Deadly) }
    };

        return xpSums;
    }

    private List<XpThresholds> CalculatePartyXpThresholds(int[] playerLevels, string difficulty)
    {
        var xpThresholds = new List<XpThresholds>();

        foreach (int level in playerLevels)
        {
            int easyThreshold = GetXpThreshold(level, "easy");
            int mediumThreshold = GetXpThreshold(level, "medium");
            int hardThreshold = GetXpThreshold(level, "hard");
            int deadlyThreshold = GetXpThreshold(level, "deadly");

            xpThresholds.Add(new XpThresholds
            {
                Level = level,
                Easy = easyThreshold,
                Medium = mediumThreshold,
                Hard = hardThreshold,
                Deadly = deadlyThreshold
            });
        }

        return xpThresholds;
    }


    private int GetXpThreshold(int level, string difficulty)
    {
        int[,] xpThresholdTable = new int[,]
        {
        {25, 50, 75, 100},
        {50, 100, 150, 200},
        {75, 150, 225, 400},
        {125, 250, 375, 500},
        {250, 500, 750, 1100},
        {300, 600, 900, 1400},
        {350, 750, 1100, 1700},
        {450, 900, 1400, 2100},
        {550, 1100, 1600, 2400},
        {600, 1200, 1900, 2800},
        {800, 1600, 2400, 3600},
        {1000, 2000, 3000, 4500},
        {1100, 2200, 3400, 5100},
        {1250, 2500, 3800, 5700},
        {1400, 2800, 4300, 6400},
        {1600, 3200, 4800, 7200},
        {2000, 3900, 5900, 8800},
        {2100, 4200, 6300, 9500},
        {2400, 4900, 7300, 10900},
        {2800, 5700, 8500, 12700}
        };

        int difficultyIndex = difficulty switch
        {
            "easy" => 0,
            "medium" => 1,
            "hard" => 2,
            "deadly" => 3,
            _ => throw new ArgumentException("Invalid difficulty value."),
        };

        int xpThreshold = xpThresholdTable[level - 1, difficultyIndex];
        return xpThreshold;
    }


    private int CalculateAdjustedXpValue(int totalXp, int numMonsters)
    {
        
        int[,] xpMultipliers = new int[,]
        {
        { 1, 1 },    
        { 2, 3 },    
        { 6, 4 },    
        { 10, 5 },   
        { 14, 6 },   
        { int.MaxValue, 7 } 
        };

        double multiplier = 1;
        for (int i = 0; i < xpMultipliers.GetLength(0); i++)
        {
            if (numMonsters <= xpMultipliers[i, 0])
            {
                multiplier = xpMultipliers[i, 1] / 2.0; 
                break;
            }
        }

        return (int)Math.Round(totalXp * multiplier, MidpointRounding.AwayFromZero);
    }


    public async Task<List<Monster>> GetMonsterList()
    {
        using (HttpClient _client = _clientFactory.CreateClient())
        {
            List<Monster> monsterList = await _monsterApiService.GetMonsterList(_client);
            return monsterList;
        }
  
    }

     public async Task<bool> SaveEncounterData(EncounterResult encounterResult)
    {
        try
        {
            await _encounterRepository.SaveEncounter(encounterResult);
            return true; 
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"EncounterService: An error occurred while saving the encounter. Exception message: {ex.Message}");
            return false; 
        }
    }

}


