using System;
using FluentAssertions;
using Xunit;

namespace Battleships.ConsoleApp.Tests
{
    public class StringToCoordinatesConverterTests
    {
        StringToCoordinatesConverter _converter = new StringToCoordinatesConverter();

        [Theory]
        [InlineData("A2sfdf")]
        [InlineData("A11")]
        [InlineData("AC")]
        [InlineData("2A")]
        public void Convert_InvalidString_ThrowException(string input)
        {
            _converter.Invoking(n => n.Convert(input))
                .Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("A1")]
        [InlineData("J8")]
        [InlineData("c10")]
        [InlineData("A2")]
        public void Convert_validString_ShouldNotThrowException(string input)
        {
            _converter.Invoking(n => n.Convert(input))
                .Should().NotThrow<ArgumentException>();
        }

        [Theory]
        [InlineData("A2", 1, 0)]
        [InlineData("J5", 4, 9)]
        [InlineData("c10", 9, 2)]
        [InlineData("B5", 4, 1)]
        [InlineData("d3", 2, 3)]
        public void Convert_ValidString_ShouldReturnCoordinates(string input, int horizontal, int vertical)
        {
            var coordinates = _converter.Convert(input);
            
            coordinates.Should().Be(new Coordinates(horizontal, vertical));
        }
    }
}