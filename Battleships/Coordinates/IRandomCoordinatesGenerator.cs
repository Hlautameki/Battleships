using System.Collections.Generic;

namespace Battleships
{
    public interface IRandomCoordinatesGenerator
    {
        Coordinates GetCoordinates();

        Coordinates GetCoordinates(IList<Coordinates> coordinates);
    }
}