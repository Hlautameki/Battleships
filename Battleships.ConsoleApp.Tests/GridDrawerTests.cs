using System;
using Xunit;
using System.IO;
using FluentAssertions;
using System.Collections.Generic;

namespace Battleships.ConsoleApp.Tests
{
    public class GridDrawerTests
    {
        private GridDrawer _gridDrawer = new GridDrawer();

        const string _emptyGridString = 
       @"   A B C D E F G H I J
1  . . . . . . . . . .
2  . . . . . . . . . .
3  . . . . . . . . . .
4  . . . . . . . . . .
5  . . . . . . . . . .
6  . . . . . . . . . .
7  . . . . . . . . . .
8  . . . . . . . . . .
9  . . . . . . . . . .
10 . . . . . . . . . ."; 

const string _threeShipsString = 
@"   A B C D E F G H I J
1  . . . . . . . . . .
2  . . . . . . . . . .
3  . . . . . . . . . .
4  . . . O O O O O . .
5  . . . . . . . . . .
6  . . O . . . O . . .
7  . . O . . . O . . .
8  . . O . . . O . . .
9  . . O . . . O . . .
10 . . . . . . . . . ."; 

const string _threeShipsHitsAndMissesString = 
@"   A B C D E F G H I J
1  . . . . . . . . . .
2  . * . . . . . . . .
3  . . . . . . . . . .
4  . * . O Ø Ø O O . .
5  . . . . . * . . * .
6  . . O . . . O . . .
7  . . O . . . O . . .
8  . . O . . . O . . .
9  . . O . . . O . . .
10 . . . . . . . . . ."; 

        [Fact]
        public void DrawGrid_DrawEmptyGrid()
        {
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                _gridDrawer.DrawGrid(new Position(0,0), new Dictionary<Coordinates, FieldType>());

                var swString = stringWriter.ToString().Trim().Replace("\r", "");
                var emptyGridStringTrimmed = _emptyGridString.Trim().Replace("\r", "");

                swString.Should().BeEquivalentTo(emptyGridStringTrimmed);
            }
        }

        [Fact]
        public void DrawGrid_DrawThreeShips()
        {
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                
                var fields = new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(3, 3), FieldType.Ship },
                    { new Coordinates(3, 4), FieldType.Ship },
                    { new Coordinates(3, 5), FieldType.Ship },
                    { new Coordinates(3, 6), FieldType.Ship },
                    { new Coordinates(3, 7), FieldType.Ship },
                    
                    { new Coordinates(5, 2), FieldType.Ship },
                    { new Coordinates(6, 2), FieldType.Ship },
                    { new Coordinates(7, 2), FieldType.Ship },
                    { new Coordinates(8, 2), FieldType.Ship },
                    
                    { new Coordinates(5, 6), FieldType.Ship },
                    { new Coordinates(6, 6), FieldType.Ship },
                    { new Coordinates(7, 6), FieldType.Ship },
                    { new Coordinates(8, 6), FieldType.Ship },
                };

                _gridDrawer.DrawGrid(new Position(0,0), fields);

                var swString = stringWriter.ToString().Trim().Replace("\r", "");
                var expectedResult = _threeShipsString.Trim().Replace("\r", "");

                swString.Should().BeEquivalentTo(expectedResult);
            }
        }

        [Fact]
        public void DrawGrid_DrawThreeShipsHitsAndMisses()
        {
            using (var stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                
                var fields = new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(1, 1), FieldType.Miss },
                    { new Coordinates(3, 1), FieldType.Miss },
                    { new Coordinates(4, 5), FieldType.Miss },
                    { new Coordinates(4, 8), FieldType.Miss },

                    { new Coordinates(3, 3), FieldType.Ship },
                    { new Coordinates(3, 4), FieldType.Hit },
                    { new Coordinates(3, 5), FieldType.Hit },
                    { new Coordinates(3, 6), FieldType.Ship },
                    { new Coordinates(3, 7), FieldType.Ship },
                    
                    { new Coordinates(5, 2), FieldType.Ship },
                    { new Coordinates(6, 2), FieldType.Ship },
                    { new Coordinates(7, 2), FieldType.Ship },
                    { new Coordinates(8, 2), FieldType.Ship },
                    
                    { new Coordinates(5, 6), FieldType.Ship },
                    { new Coordinates(6, 6), FieldType.Ship },
                    { new Coordinates(7, 6), FieldType.Ship },
                    { new Coordinates(8, 6), FieldType.Ship },
                };

                _gridDrawer.DrawGrid(new Position(0,0), fields);

                var swString = stringWriter.ToString().Trim().Replace("\r", "");
                var expectedResult = _threeShipsHitsAndMissesString.Trim().Replace("\r", "");

                swString.Should().BeEquivalentTo(expectedResult);
            }
        }
    }
}
