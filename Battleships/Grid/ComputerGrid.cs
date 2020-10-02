namespace Battleships
{
    public class ComputerGrid<UserGridType> : Grid<ComputerGridType>, IGrid<ComputerGridType>
    {
        public ComputerGrid(IShipBuilder shipBuilder, GameSettings gameSettings) 
            : base(shipBuilder, gameSettings)
        {
        }
    }
}