using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;
using System.Linq;

namespace Battleships.Tests
{
    public class AITests
    {
        private AI _ai;
        GameSettings _gameSettings = new GameSettings();
        IRandomCoordinatesGenerator _rcg = Substitute.For<IRandomCoordinatesGenerator>();
        Dictionary<Coordinates, FieldType> _enemyGrid = new Dictionary<Coordinates, FieldType>();

        IList<Dictionary<Coordinates, FieldType>> _ships = new List<Dictionary<Coordinates, FieldType>>();

        public AITests()
        {
            var services = new ServiceCollection();
            services.AddTransient<AI>();
            services.AddSingleton<IRandomCoordinatesGenerator>(_rcg);
            services.AddSingleton<GameSettings>();
            var serviceProvider = services.BuildServiceProvider();

            _ai = serviceProvider.GetService<AI>();
        }

        [Fact]
        public void GetCoordinates_ThereAreNotAlreadyTakenShoots_ReturnRandomCoordinates()
        {
            var coordinates = new Coordinates(1, 3);
            _rcg.GetCoordinates().Returns(coordinates);

            _ai.GetShootCoordinates(_enemyGrid).Should().Be(coordinates);
        }

        [Fact]
        public void GetShootCoordinates_ThereAreAlreadyTakenShoots_ThereShouldNotBeRepetition()
        {
            var coordinates1 = new Coordinates(3, 3);
            var coordinates2 = new Coordinates(3, 2);
            _enemyGrid[coordinates1] = FieldType.Miss;
            _rcg.GetCoordinates().Returns(coordinates1, coordinates2);

            _ai.GetShootCoordinates(_enemyGrid).Should().Be(coordinates2);

            _rcg.Received(2).GetCoordinates();
        }

        public static IEnumerable<object[]> Data()
        {

            yield return new object[] // ThereAreAlreadyTakenSuccessfulShoots_ReturnsContiguousField
            {
                new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(7, 7), FieldType.Miss },
                    { new Coordinates(8, 3), FieldType.Hit }
                },
                new List<Coordinates>
                {
                    new Coordinates(8,4),
                    new Coordinates(8,2),
                    new Coordinates(7,3),
                    new Coordinates(9,3),
                }
            };
            yield return new object[] // ThereAreAlreadyTakenSuccessfulShoots_ContiguousFieldDontExceedGridBorders()
            {
                new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(7, 7), FieldType.Miss },
                    { new Coordinates(9, 9), FieldType.Hit }
                },
                new List<Coordinates>
                {
                    new Coordinates(8,9),
                    new Coordinates(9,8),
                }
            };

            yield return new object[] // ThereAreAlreadyTakenSuccessfulShoots_ContiguousFieldDontExceedGridBorders()
            {
                new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(7, 7), FieldType.Miss },
                    { new Coordinates(0, 0), FieldType.Hit }
                },
                new List<Coordinates>
                {
                    new Coordinates(0, 1),
                    new Coordinates(1, 0),
                }
            };

            yield return new object[] // ThereAreAlreadyTakenSuccessfulAndMiesseShoots_ReturnsContiguousFieldButNotMissedOne
            {
                new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(8, 4), FieldType.Miss },
                    { new Coordinates(8, 3), FieldType.Hit }
                },
                new List<Coordinates>
                {
                    new Coordinates(8,2),
                    new Coordinates(7,3),
                    new Coordinates(9,3),
                }
            };

            yield return new object[] // TwoHits_FirstSurroundedBySeeFields_ShouldReturnContiguousFieldToSecondOne
            {
                new Dictionary<Coordinates, FieldType>()
                {
                    { new Coordinates(8, 2), FieldType.Miss },
                    { new Coordinates(7, 3), FieldType.Miss },
                    { new Coordinates(9, 3), FieldType.Miss },
                    { new Coordinates(8, 3), FieldType.Hit },
                    { new Coordinates(8, 4), FieldType.Hit }
                },
                new List<Coordinates>
                {
                    new Coordinates(8,5),
                    new Coordinates(7,4),
                    new Coordinates(9,4),
                }
            };
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetShootCoordinates_ThereAreAlreadyTakenSuccessfulShoots_ReturnsContiguousField(Dictionary<Coordinates, FieldType> enemyGrid, List<Coordinates> expectedCoordinates)
        {
            _rcg.GetCoordinates(Arg.Is<List<Coordinates>>(n => n.Intersect(expectedCoordinates).Count() == expectedCoordinates.Count))
                .Returns(expectedCoordinates.First());

            var receivedCoordinates = _ai.GetShootCoordinates(enemyGrid);

            expectedCoordinates.Contains(receivedCoordinates).Should().BeTrue();
        }
    }
}