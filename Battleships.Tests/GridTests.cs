using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Xunit;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Battleships.Tests
{
    public class GridTests
    {
        IGrid<UserGridType> _grid;
        IShipBuilder _shipBuilder;
        IFixture _fixture = new Fixture();

        GameSettings _gameSettings = new GameSettings();

        public GridTests()
        {
            _shipBuilder = Substitute.For<IShipBuilder>();
            var services = new ServiceCollection();
            services.AddTransient<IGrid<UserGridType>, Grid<UserGridType>>();
            services.AddSingleton<GameSettings>(_gameSettings);
            services.AddSingleton<IShipBuilder>(_shipBuilder);
            var serviceProvider = services.BuildServiceProvider();

            _grid = serviceProvider.GetService<IGrid<UserGridType>>();
        }

        [Fact]
        public void GetAllFields_FieldsNumberEqualsFieldsNumberFromGameSettings()
        {
            _fixture.Customizations.Add(
                new RandomNumericSequenceGenerator(0, 9));
            for (int i = 0; i < 20; i++)
            {
                var ship1 = _fixture.CreateMany<Coordinates>(5).ToList();
                var ship2 = _fixture.CreateMany<Coordinates>(4).ToList();
                var ship3 = _fixture.CreateMany<Coordinates>(4).ToList();
                _shipBuilder.Build(Arg.Is<int>(x => x == 4 || x == 5), Arg.Any<IEnumerable<Coordinates>>())
                    .Returns(ship1, ship2, ship3);
                _grid.Initialize();

                Dictionary<Coordinates, FieldType> allFields = _grid.GetAllFields();

                allFields.Count.Should().Be((int)Math.Pow(_gameSettings.GridSize, 2));
            }
        }

        [Fact]
        public void GetShips_ShouldReturnShipsFromShipBuilder()
        {
            var ship1 = new List<Coordinates>
            {
                new Coordinates(2,5),
                new Coordinates(2,6),
                new Coordinates(2,7),
                new Coordinates(2,8),
                new Coordinates(2,9)
            };
            var ship2 = new List<Coordinates>()
            {
                new Coordinates(4,3),
                new Coordinates(5,3),
                new Coordinates(6,3),
                new Coordinates(7,3)
            };
            var ship3 = new List<Coordinates>()
            {
                new Coordinates(4,7),
                new Coordinates(5,7),
                new Coordinates(6,7),
                new Coordinates(7,7)
            };
            _shipBuilder.Build(Arg.Is<int>(x => x == 4 || x == 5), Arg.Any<IEnumerable<Coordinates>>())
                .Returns(ship1, ship2, ship3);
            _grid.Initialize();

            Dictionary<Coordinates, FieldType> allFields = _grid.GetAllFields();

            allFields.Where(n => n.Value == FieldType.Ship).Select(n => n.Key)
                .Should().BeEquivalentTo(ship1.Concat(ship2).Concat(ship3));
        }
    }
}