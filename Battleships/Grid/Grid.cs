using System.Collections.Generic;
using System.Linq;

namespace Battleships
{
    public class Grid<GridTypeResolver> : IGrid<UserGridType>
    {
        List<List<Coordinates>> _ships = new List<List<Coordinates>>();
        Dictionary<Coordinates, FieldType> _fields = new Dictionary<Coordinates, FieldType>();
        private readonly IShipBuilder _shipBuilder;
        private readonly GameSettings _gameSettings;

        public Grid(IShipBuilder shipBuilder, GameSettings gameSettings)
        {
            this._shipBuilder = shipBuilder;
            this._gameSettings = gameSettings;
        }

        public Dictionary<Coordinates, FieldType> GetAllFields()
        {
            return _fields;
        }
        public void Initialize()
        {
            foreach (var shipType in _gameSettings.ShipTypes)
            {
                var ship = _shipBuilder.Build(shipType, _ships.SelectMany(n => n));

                _ships.Add(ship);
            }

            for (int i = 0; i < _gameSettings.GridSize; i++)
            {
                for (int j = 0; j < _gameSettings.GridSize; j++)
                {
                    if (!_ships.SelectMany(n => n).Contains(new Coordinates(i, j)))
                    {
                        _fields[new Coordinates(i, j)] = FieldType.See;
                    }
                    else
                    {
                        _fields[new Coordinates(i, j)] = FieldType.Ship;
                    }
                }
            }
        }

        public IList<Dictionary<Coordinates, FieldType>> GetShips()
        {
            var ships = new List<Dictionary<Coordinates, FieldType>>();

            foreach (var ship in _ships)
            {
                var shipDic = new Dictionary<Coordinates, FieldType>();
                foreach (var item in ship)
                {
                    shipDic[item] = _fields[item];
                }
                ships.Add(shipDic);
            }

            return ships;
        }

        public void SetFieldType(Coordinates coordinates, FieldType fieldType)
        {
            _fields[coordinates] = fieldType;
        }
    }
}