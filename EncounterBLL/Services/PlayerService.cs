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
}