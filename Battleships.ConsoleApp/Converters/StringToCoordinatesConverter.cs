using System;
using System.Text.RegularExpressions;

namespace Battleships.ConsoleApp
{
    public class StringToCoordinatesConverter
    {
        public Coordinates Convert(string input)
        {
            if (!Regex.IsMatch(input, "^[a-zA-Z][1-9]($|0)"))
                throw new ArgumentException("Incorrect input format. Please try again.");
            
            var letter = input[0];
            var number = input.Substring(1);

            int horizontal = Int32.Parse(number) - 1;
            int vertical = LetterToInt(letter) - 1;

            return new Coordinates(horizontal, vertical);
        }

        private int LetterToInt(char letter)
        {
            return char.ToUpper(letter) - 64;
        }
    }
}