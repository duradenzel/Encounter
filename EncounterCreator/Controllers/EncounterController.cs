using EncounterCreator.ViewModels;
using EncounterInterfaces;
using EncounterModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            
            _encounterService = encounterService;
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


       [HttpPost]
        public async Task<IActionResult> SaveEncounter()
        {
            try
            {
                // Check if there are monsters in the encounter
                if (Encounter?.Monsters?.Any() == true)
                {
                    // Convert the EncounterResult to a list of Monster objects
                    List<Monster> monsters = Encounter.Monsters
                        .Select(m => new Monster
                        {
                            Name = m.Name,
                            Size = m.Size,
                            Type = m.Type,
                            CR = m.CR,
                            ExperiencePoints = m.ExperiencePoints
                            // Add any other properties you need
                        })
                        .ToList();

                    // Call your service to handle the monsters and save them to the database
                    await _encounterService.SaveEncounter(monsters);

                    // Optionally, you can clear the monsters in the encounter after saving
                    Encounter.Monsters.Clear();

                    // You can return a success response if needed
                    return View("GenerateEncounter");
                }
                else
                {
                    // Handle the case when there are no monsters in the encounter
                    return View("GenerateEncounter", "No monsters in the encounter to save.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return View("GenerateEncounter", $"Error saving encounter: {ex.Message}");
            }
        }
    
    }
}
