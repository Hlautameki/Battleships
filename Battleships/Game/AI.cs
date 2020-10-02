using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships
{
    public class AI
    {
        IRandomCoordinatesGenerator _rcg;
        private readonly GameSettings _gameSettings;

        public AI(IRandomCoordinatesGenerator randomCoordinatesGenerator, GameSettings gameSettings)
        {
            _rcg = randomCoordinatesGenerator;
            _gameSettings = gameSettings;
        }
        public Coordinates GetShootCoordinates(Dictionary<Coordinates, FieldType> _enemyGrid)
        {
            Coordinates coordinates;

            foreach (var item in _enemyGrid.Where(n => n.Value == FieldType.Hit))
            {
                var result = GetContiguosCoordinates(item.Key, _enemyGrid.Select(n => n.Key));
                if (result.Item1)
                    return result.Item2;
            }

            do
            {
                coordinates = _rcg.GetCoordinates();
            } while (_enemyGrid.ContainsKey(coordinates));

            return coordinates;
        }

        private (bool, Coordinates) GetContiguosCoordinates(Coordinates hit, IEnumerable<Coordinates> knownFields)
        {
            var coordinatesList = new List<Coordinates>();

            if (TryGetCoordinatesPropertyMinus(hit.Vertical, out int moveLeft) && !knownFields.Contains(new Coordinates(hit.Horizontal, moveLeft)))
                coordinatesList.Add(new Coordinates(hit.Horizontal, moveLeft));
            if (TryGetCoordinatesPropertyMinus(hit.Horizontal, out int moveDown) && !knownFields.Contains(new Coordinates(moveDown, hit.Vertical)))
                coordinatesList.Add(new Coordinates(moveDown, hit.Vertical));
            if (TryGetCoordinatesPropertyPlus(hit.Vertical, out int moveRight) && !knownFields.Contains(new Coordinates(hit.Horizontal, moveRight)))
                coordinatesList.Add(new Coordinates(hit.Horizontal, moveRight));
            if (TryGetCoordinatesPropertyPlus(hit.Horizontal, out int moveUp) && !knownFields.Contains(new Coordinates(moveUp, hit.Vertical)))
                coordinatesList.Add(new Coordinates(moveUp, hit.Vertical));

            if (coordinatesList.Count == 0)
                return (false, new Coordinates());

            return (true, _rcg.GetCoordinates(coordinatesList));
        }

        private bool TryGetCoordinatesPropertyPlus(int property, out int result)
        {
            if ((property + 1) > (_gameSettings.GridSize - 1))
            {
                result = -1;
                return false;
            }

            result = property + 1;
            return true;
        }

        private bool TryGetCoordinatesPropertyMinus(int property, out int result)
        {
            if ((property - 1) < 0)
            {
                result = -1;
                return false;
            }

            result = property - 1;
            return true;
        }
    }
}