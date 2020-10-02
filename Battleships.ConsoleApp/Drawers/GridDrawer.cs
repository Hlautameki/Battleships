using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Battleships.ConsoleApp
{
    public class GridDrawer
    {
        public void DrawGrid(Position position, Dictionary<Coordinates, FieldType> fields)
        {
            CursorTop = position.Top;
            CursorLeft = position.Left;
            WriteLine($"   A B C D E F G H I J");
            var letterToNumberDict = new Dictionary<char,int>();
            var numberToLetterDict = new Dictionary<int, char>()
            {
                {0, 'A'},
                {1, 'B'},
                {2, 'C'},
                {3, 'D'},
                {4, 'E'},
                {5, 'F'},
                {6, 'G'},
                {7, 'H'},
                {8, 'I'},
                {9, 'J'},
            };
            for (int i = 0; i < 10; i++)
            {
                CursorLeft = position.Left;
                var gridRow = new StringBuilder();
                //gridRow.Append($"{numberToLetterDict[i]} ");
                gridRow.Append($"{i+1}");
                if (gridRow.Length < 2)
                    gridRow.Append(" ");
                for (int j = 0; j < 10; j++)
                {
                    gridRow.Append(" ");
                    fields.TryGetValue(new Coordinates(i, j), out FieldType field);
                    gridRow.Append(field.ToChar());
                }
                WriteLine(gridRow.ToString());
            }
        }
    }
}