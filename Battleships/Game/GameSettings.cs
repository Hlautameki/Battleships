using System.Collections.Generic;

namespace Battleships
{
    public class GameSettings
    {
        public int GridSize => 10;

        public string DefaultUserName => "UserPlayer";

        public string DefaultComputerPlayerName => "ComputerPlayer";

        public string GameName = "Battleships";

        public readonly List<int> ShipTypes = new List<int>{5, 4, 4};
    }    
}