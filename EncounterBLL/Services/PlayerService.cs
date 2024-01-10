using EncounterBLL;
using EncounterInterfaces;
using EncounterModels;
using EncounterBLL.Factories;
using MySqlX.XDevAPI;

public class PlayerService
{
    private readonly DataAccessFactory _dataAccessFactory;
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(DataAccessFactory dataAccessFactory)
    {
        _dataAccessFactory = dataAccessFactory;
        _playerRepository = _dataAccessFactory.GetPlayerRepository();
    }

    public List<Player> GetAllPlayers(){
        List<Player> players = _playerRepository.GetAllPlayers();
        return players;
    }

    public List<EncounterResult> GetEncountersByPlayerId(int id){
        List<EncounterResult> encounters = _playerRepository.GetEncountersByPlayerId(id);
        return encounters;
    }
}