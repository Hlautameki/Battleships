using System.Collections.Generic;

namespace Battleships
{
    public interface IShipBuilder
    {
        List<Coordinates> Build(int shipLength, IEnumerable<Coordinates> existingShips);
    }
}