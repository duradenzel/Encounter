using EncounterCreator.ViewModels;
using EncounterInterfaces;
using EncounterModels;
using Microsoft.AspNetCore.Mvc;

namespace EncounterCreator.Controllers
{
    public class EncounterController : Controller
    {
        private readonly EncounterService _encounterService;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _httpClient;
        public EncounterResult Encounter = new();


        public EncounterController(EncounterService encounterService, IHttpClientFactory httpClientFactory)
        {
            _encounterService = encounterService;
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient();
        }

        public IActionResult Index() { return View("GenerateEncounter"); }

        [HttpPost]
        public async Task<IActionResult> GenerateEncounter(int partySize, int playerLevel, string difficulty = "easy")
        {

            Encounter = await _encounterService.GenerateEncounter(partySize, playerLevel, difficulty, _httpClient);
            
            return View("GenerateEncounter",Encounter);

        }

        public async Task<List<Monster>> GetMonsterList() {
            List<Monster> monsterList = await _encounterService.GetMonsterList(_httpClient);
            return monsterList;
        }

     

        public ActionResult AddMonsterToEncounter(Monster monster)
        {
            
            Encounter.Monsters.Add(monster);
            Encounter.TotalExp += monster.ExperiencePoints; 
            

            return PartialView("_EncounterDetails", Encounter);
        }

    
    }
}
