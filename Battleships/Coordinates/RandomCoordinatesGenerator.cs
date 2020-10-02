using System;
using System.Collections.Generic;

namespace Battleships
{
    public class RandomCoordinatesGenerator : IRandomCoordinatesGenerator
    {
        private readonly GameSettings _gameSettings;
        Random _rnd = new Random();

        public RandomCoordinatesGenerator(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        public Coordinates GetCoordinates()
        {
            return new Coordinates(_rnd.Next(0, _gameSettings.GridSize - 1), _rnd.Next(0, _gameSettings.GridSize - 1));
        }

        public Coordinates GetCoordinates(IList<Coordinates> coordinates)
        {
            if (coordinates == null || coordinates.Count == 0)
                return GetCoordinates();
                
            var index = _rnd.Next(0, coordinates.Count - 1);
            return coordinates[index];
        }
    }
}