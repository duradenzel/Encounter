using EncounterCreator.ViewModels;
using EncounterInterfaces;
using EncounterModels;
using Microsoft.AspNetCore.Mvc;

namespace EncounterCreator.Controllers
{
    public class EncounterController : Controller
    {
        private readonly IEncounterService _encounterService;
        public EncounterResult Encounter = new();

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        public IActionResult Index() { return View("GenerateEncounter"); }

        [HttpPost]
        public async Task<IActionResult> GenerateEncounter(int partySize, int playerLevel, string difficulty = "easy")
        {                 
            Encounter = await _encounterService.GenerateEncounter(partySize, playerLevel, difficulty);
            
            return View("GenerateEncounter",Encounter);

        }

        public async Task<List<Monster>> GetMonsterList() {
            List<Monster> monsterList = await _encounterService.GetMonsterList();
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
