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
        private readonly PlayerService _playerService;

        private PlayerViewModel playerViewModel;
        


        public EncounterController(EncounterService encounterService, PlayerService playerService)
        {
            
            _encounterService = encounterService;
            _playerService = playerService;
            playerViewModel = new PlayerViewModel();
        
        }

        public IActionResult Index() { 


            playerViewModel.Players = _playerService.GetAllPlayers();
            playerViewModel.PlayerEncounters = _playerService.GetEncountersByPlayerId(1);

            return View("GenerateEncounter", playerViewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> GenerateEncounter(int partySize, int playerLevel, string difficulty)
        {

            EncounterResult Encounter = await _encounterService.GenerateEncounter(partySize, playerLevel, difficulty);
            playerViewModel.Encounter = Encounter;
            playerViewModel.Players = _playerService.GetAllPlayers();
            playerViewModel.PlayerEncounters = _playerService.GetEncountersByPlayerId(1);

            return View("GenerateEncounter", playerViewModel);

        }

        public async Task<List<Monster>> GetMonsterList() {
            List<Monster> monsterList = await _encounterService.GetMonsterList();
            return monsterList;
        }

      

      //TODO: Fix bug: CR value cannot be a float value (cr 0.25 , 0.5 etc). gives null reference when passed from view
        [HttpPost]  
        public async Task<IActionResult> SaveEncounter([FromBody] EncounterResult encounterResult)
        {
            var saveResult = await _encounterService.SaveEncounterData(encounterResult);

            if (saveResult)
            {
                return Ok(new { Message = "Encounter saved successfully." });
            }
            else
            {
                return StatusCode(500, new { Message = "EncounterController: An error occurred while saving the encounter." });
            }
        }

        public async Task<IActionResult> GetEncounter(int id = 14){

            EncounterResult encounter = _playerService.GetEncounter(14);
            encounter.XpSums = _encounterService.CalculateAllXpSums(_encounterService.CalculatePartyXpThresholds(encounter.PlayerLevels, encounter.Difficulty));
            playerViewModel.Encounter = encounter;

            
            playerViewModel.Players = _playerService.GetAllPlayers();
            playerViewModel.PlayerEncounters = _playerService.GetEncountersByPlayerId(1);

            return View("GenerateEncounter", playerViewModel);
        }

    }
}
