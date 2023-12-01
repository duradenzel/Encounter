using EncounterDAL;
using EncounterModels;
using Microsoft.Extensions.Caching.Memory;

namespace Testing;

[TestClass]
public class MonsterApiTests
{
     private HttpClient _httpClient;
    private IMemoryCache _memoryCache;

    public MonsterApiTests()
    {
        _httpClient = new HttpClient();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    
    [TestMethod]
    public async Task GetMonsterList_ReturnsAllMonsters(){
        var api = new MonsterApi(_memoryCache);

        var monsters = await api.GetMonsterList(_httpClient);

        Assert.IsNotNull(monsters, "The monster list shuold not be null");
        Assert.IsTrue(monsters.Count > 0, "There should be at least one monster in the result");

    }

     [TestMethod]
    public async Task GetMonsterWithXP_ReturnsSelectedMonsters(){
        var api = new MonsterApi(_memoryCache);

        var monsters = await api.GetMonsters(3000,_httpClient);

        Assert.IsNotNull(monsters, "The monster list should not be null");
        Assert.IsTrue(monsters.Count > 0, "There should be at least one monster in the result");

    }

    // [TestMethod]
    // public async Task GetMonsters_CacheContainsData_ShouldReturnSelectedMonstersFromCache()
    // {
        
    //     var api = new MonsterApi(_memoryCache);
        

    //     var cacheMonsters = new List<Monster> {
    //         new Monster{
    //             Name = "Aboleth",
    //             CR = 10,
    //             ExperiencePoints = 6000
    //         },
    //             new Monster{
    //             Name = "Assassin",
    //             CR = 4,
    //             ExperiencePoints = 600
    //         }
    //     };
    //     _memoryCache.Set("AllMonsters", cacheMonsters, TimeSpan.FromHours(24));

    //     var monsters = await api.GetMonsters(10000, _httpClient);

    //     Assert.IsNotNull(monsters, "The result should not be null");
    // }

}