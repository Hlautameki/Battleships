using System;
using System.Linq;

namespace Battleships
{
    public class Game
    {
        private GameStatus _gameStatus;
        private readonly GameSettings _gameSettings;
        private readonly AI _ai;

        public Game(GameStatus gameStatus, GameSettings gameSettings, AI ai)
        {
            _gameStatus = gameStatus;
            _gameSettings = gameSettings;
            _ai = ai;
        }

        public GameStatus GetStatus()
        {
            return _gameStatus;
        }

        public GameStatus Shoot(Coordinates coordinates)
        {
            Shoot<ComputerGridType>(_gameStatus.ComputerGrid, coordinates);

            return _gameStatus;
        }

        private void Shoot<T>(IGrid<T> grid, Coordinates coordinates) where T : GridTypeResolver
        {
            if (coordinates.Horizontal > _gameSettings.GridSize - 1
                            || coordinates.Vertical > _gameSettings.GridSize - 1)
            {
                throw new ArgumentException("Invalid coordinates. Please try again.");
            }

            var fieldTypeCheck = grid.GetAllFields()[coordinates];
            if (fieldTypeCheck == FieldType.Hit || fieldTypeCheck == FieldType.Miss)
                throw new ArgumentException("Shot has been already taken at that square. Please choose another one.");

            var fieldType = grid.GetAllFields()[coordinates];

            if (fieldType == FieldType.See)
                grid.SetFieldType(coordinates, FieldType.Miss);
            else if (fieldType == FieldType.Ship)
                grid.SetFieldType(coordinates, FieldType.Hit);
        }

        public GameStatus Shoot()
        {
            var coordinates = _ai.GetShootCoordinates(_gameStatus.UserGrid.GetAllFields()
                .Where(n => n.Value != FieldType.Ship && n.Value != FieldType.See)
                .ToDictionary(k => k.Key, k => k.Value));

            Shoot<UserGridType>(_gameStatus.UserGrid, coordinates);

            return _gameStatus;
        }

        public string GetWinner()
        {
            if (_gameStatus.ComputerGrid.GetShips().Any() &&
                _gameStatus.ComputerGrid.GetShips().SelectMany(n => n.Values).All(n => n == FieldType.Hit))
                return _gameStatus.UserName;
            else if (_gameStatus.UserGrid.GetShips().Any() &&
                _gameStatus.UserGrid.GetShips().SelectMany(n => n.Values).All(n => n == FieldType.Hit))
                return _gameStatus.ComputerPlayerName;

            return default;
        }
    }
}