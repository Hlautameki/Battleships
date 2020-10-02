namespace Battleships
{
    public class GameStatus
    {
        public IGrid<UserGridType> UserGrid { get; set; }
        public IGrid<ComputerGridType> ComputerGrid { get; set; }

        public string GameName => _gameSettings.GameName;
        private readonly GameSettings _gameSettings; 

        public string ComputerPlayerName => _gameSettings.DefaultComputerPlayerName;
        public string UserName => _gameSettings.DefaultUserName;

        public GameStatus(IGrid<UserGridType> userGrid, IGrid<ComputerGridType> pcPlayerGrid, GameSettings gameSettings)
        {
            UserGrid = userGrid;
            UserGrid.Initialize();
            ComputerGrid = pcPlayerGrid;
            _gameSettings = gameSettings;
            ComputerGrid.Initialize();
        }
    }
}
