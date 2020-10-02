using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Battleships.ConsoleApp
{
    public class ShipsStatusDrawer
    {
        public void Draw(Position position, IList<Dictionary<Coordinates, FieldType>> shipsDics)
        {
            CursorTop = position.Top;
            CursorLeft = position.Left;
            List<List<FieldType>> ships = new List<List<FieldType>>();
            foreach (var item in shipsDics)
            {
                ships.Add(item.Select(n=>n.Value).ToList());
            }
            var shipsOrdered = ships.OrderByDescending(n => n.Count).ToList();
            WriteLine($"Battleship: {new string (shipsOrdered[0].OrderByDescending(n =>n).Select(n =>  n.ToChar()).ToArray())}");
            CursorLeft = position.Left;
            WriteLine($"Destroyer:  {new string (shipsOrdered[1].OrderByDescending(n =>n).Select(n =>  n.ToChar()).ToArray())}");
            CursorLeft = position.Left;
            WriteLine($"Destroyer:  {new string (shipsOrdered[2].OrderByDescending(n =>n).Select(n =>  n.ToChar()).ToArray())}");
        }
    }
}