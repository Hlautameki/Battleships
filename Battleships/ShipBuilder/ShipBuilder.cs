using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships
{
    public class ShipBuilder : IShipBuilder
    {
        Random _rnd = new Random();
        IRandomCoordinatesGenerator _rcd;

        private readonly GameSettings _gameSettings;

        public ShipBuilder(GameSettings gameSettings, IRandomCoordinatesGenerator rcd)
        {
            this._gameSettings = gameSettings;
            _rcd = rcd;
        }

        public List<Coordinates> Build(int shipLength, IEnumerable<Coordinates> existingShips)
        {
            List<Coordinates> coordinates = new List<Coordinates>();
            var initialPosition = GetRandomCoordinates(existingShips);
            var direction = GetRandomDirection();
            coordinates.Add(initialPosition);
            for (int i = 1; i < shipLength; i++)
            {
                if (direction == Direction.Horizontally)
                {
                    if (TryGetNewCoordinate(coordinates.Select(n=>n.Horizontal).ToList(), out int horizontalCoordinate))
                        coordinates.Add(new Coordinates(horizontalCoordinate, coordinates[i-1].Vertical));
                }
                else
                {
                    if (TryGetNewCoordinate(coordinates.Select(n => n.Vertical).ToList(), out int verticalCoordinate))
                        coordinates.Add(new Coordinates(coordinates[i-1].Horizontal, verticalCoordinate));
                }
            }

            if (coordinates.Count != shipLength || coordinates.Except(existingShips).Count() != coordinates.Count)
                coordinates = Build(shipLength, existingShips);

            if (direction == Direction.Horizontally)
                coordinates = coordinates.OrderBy(n => n.Horizontal).ToList();            
            
            if (direction == Direction.Vertically)
                coordinates = coordinates.OrderBy(n => n.Vertical).ToList();            

            return coordinates;
        }

        bool TryGetNewCoordinate(List<int> currentCoordinates, out int newCoordinate)
        {
            if (currentCoordinates.Max() + 1 < _gameSettings.GridSize)
            {
                newCoordinate = currentCoordinates.Max() + 1;
                return true;
            }
            else if (currentCoordinates.Min() - 1 > 0)
            {
                newCoordinate = currentCoordinates.Min() - 1;
                return true;
            }
            newCoordinate = -1;
            return false;
        }

        private Coordinates GetRandomCoordinates(IEnumerable<Coordinates> existingShips)
        {
            Coordinates coordinates;

            do
            {
                coordinates = _rcd.GetCoordinates();
            } while (existingShips.Contains(coordinates));

            return coordinates;
        }

        private Direction GetRandomDirection()
        {
            Array values = Enum.GetValues(typeof(Direction));
            return (Direction)values.GetValue(_rnd.Next(values.Length));
        }

        enum Direction
        {
            Horizontally,
            Vertically
        }
    }
}