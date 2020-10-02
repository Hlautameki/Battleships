#define CODE_ANALYSIS
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Repeat;

namespace Battleships.Tests
{
    public class ShipBuilderTests
    {
        private IShipBuilder _builder;
        private List<Coordinates> _existingShips = new List<Coordinates>();

        public ShipBuilderTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<GameSettings>();
            services.AddTransient<IShipBuilder, ShipBuilder>();
            services.AddTransient<IRandomCoordinatesGenerator, RandomCoordinatesGenerator>();
            var serviceProvider = services.BuildServiceProvider();

            _builder = serviceProvider.GetService<IShipBuilder>();
        }

        private GameSettings _gameSettings = new GameSettings();

        [Theory]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(5)]
        [InlineData(4)]
        [InlineData(3)]
        public void Build_ShouldReturnCoordinatesNumberEqualShipLength(int shipLength)
        {
            _builder.Build(shipLength, _existingShips).Count.Should().Be(shipLength);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(5)]
        [InlineData(4)]
        [InlineData(3)]
        public void Start_ShipCoordinatesShouldBeNextToEachOtherAndInOneDirection(int shipLength)
        {
            var ship = _builder.Build(shipLength, _existingShips);
            CoordinatesAreNextToEachOther(ship).Should().BeTrue();
        }

        [Theory]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(5)]
        [InlineData(4)]
        [InlineData(3)]
        public void Start_ThereShouldBeNoRepetitionInDirectionalCoordinates(int shipLength)
        {
            var ship = _builder.Build(shipLength, _existingShips);
            ThereIsNoRepetitionInDirectionalCoordinates(ship).Should().BeTrue();
        }


        [Theory]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(5)]
        [InlineData(4)]
        [InlineData(3)]
        public void Start_ShipCoordinatesShouldBeInsideGrid(int shipLength)
        {
            foreach (var coordinates in _builder.Build(shipLength, _existingShips))
            {
                coordinates.Horizontal.Should().BeLessThan(_gameSettings.GridSize);
                coordinates.Vertical.Should().BeLessThan(_gameSettings.GridSize);
            }
        }

        [Theory]
        [Repeat(10)] // Requires iterationNumber parameter to test it completely
        [SuppressMessage("Microsoft.Performance", "xUnit1026:ReviewUnusedParameters", MessageId = "iterationNumber")]
        public void Start_ThereShouldBeNoRepetitionInCoordinatesBetweenShips(int iterationNumber)
        {
            foreach (var shipLength in new int[] {5, 4, 4})
            {
                _existingShips.AddRange(_builder.Build(shipLength, _existingShips));
            }
            
            ThereIsNoRepetitionInCoordinates(_existingShips).Should().BeTrue();
        }

        bool CoordinatesAreNextToEachOther(IList<Coordinates> coordinates)
        {
            var horizontal = coordinates.Select(n => n.Horizontal).ToList();
            var vertical = coordinates.Select(n => n.Vertical).ToList();

            return CoordinatesAreInSequence(horizontal) || CoordinatesAreInSequence(vertical);
        }

        bool CoordinatesAreInSequence(IList<int> coordinates)
        {
            for (int i = 0; i < coordinates.Count - 1; i++)
            {
                if (coordinates[i] + 1 != coordinates[i+ 1])
                    return false;
            }

            return true;
        }
        private bool ThereIsNoRepetitionInDirectionalCoordinates(List<Coordinates> ship)
        {
            return !(ship.GroupBy(x => x.Horizontal)
              .Where(g => g.Count() > 1)
              .Any() && 
              ship.GroupBy(x => x.Vertical)
              .Where(g => g.Count() > 1)
              .Any());
        }

        private bool ThereIsNoRepetitionInCoordinates(List<Coordinates> ship)
        {
            return !(ship.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Any());
        }
    }
}