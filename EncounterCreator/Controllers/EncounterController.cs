using System.Diagnostics;
using EncounterCreator.ViewModels;
using EncounterInterfaces;
using EncounterModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace EncounterCreator.Controllers
{
    public class EncounterController : Controller
    {
        private readonly EncounterService _encounterService;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _httpClient;
        


        public EncounterController(EncounterService encounterService, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            
            _encounterService = encounterService;
        
        }

        public IActionResult Index() { return View("GenerateEncounter"); }

        [HttpPost]
        public async Task<IActionResult> GenerateEncounter(int partySize, int playerLevel, string difficulty)
        {

            EncounterResult Encounter = await _encounterService.GenerateEncounter(partySize, playerLevel, difficulty, _httpClient);
            
            return View("GenerateEncounter",Encounter);

        }

        public async Task<List<Monster>> GetMonsterList() {
            List<Monster> monsterList = await _encounterService.GetMonsterList(_httpClient);
            return monsterList;
        }

      
        [HttpPost]  
        public async Task<IActionResult> SaveEncounter([FromBody] EncounterResult encounterResult)
        {
            try
            {
                _encounterService.SaveEncounterData(encounterResult);
                return Ok(new { Message = "Encounter saved successfully." });
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred while saving the encounter. Exception message: {ex.Message}");

                return StatusCode(500, new { Message = "An error occurred while saving the encounter." });
            }
        }

    }
}
