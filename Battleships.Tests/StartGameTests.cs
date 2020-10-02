using System;
using FluentAssertions;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Battleships.Tests
{
    public class StartGameTests
    {
        private Game game;

        private GameSettings _gameSettings = new GameSettings();

        public StartGameTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<GameSettings>(_gameSettings);
            services.AddTransient<IShipBuilder, ShipBuilder>();
            services.AddTransient<Game>();
            services.AddTransient<IGrid<UserGridType>, Grid<UserGridType>>();
            services.AddTransient<IGrid<ComputerGridType>, ComputerGrid<ComputerGridType>>();
            services.AddTransient<GameStatus>();
            services.AddTransient<AI>();
            services.AddTransient<IRandomCoordinatesGenerator, RandomCoordinatesGenerator>();
            var serviceProvider = services.BuildServiceProvider();

            game = serviceProvider.GetService<Game>();
        }

        private GameStatus Start()
        {
            return game.GetStatus();
        }

        [Fact]
        public void ShouldCreateGameObject()
        {
            game.Should().NotBeNull();
        }

        [Fact]
        public void Start_ShouldReturnStatus()
        {
            var gameStatus = Start();
            gameStatus.Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnGameStatusWithGameName()
        {
            var gameStatus = Start();
            gameStatus.GameName.Should().Be(_gameSettings.GameName);
        }

        [Fact]
        public void ShouldReturnGameStatusWithComputerPlayerName()
        {
            var gameStatus = Start();
            gameStatus.ComputerPlayerName.Should().Be(_gameSettings.DefaultComputerPlayerName);
        }

        [Fact]
        public void ShouldReturnGameStatusWithDefaultUserName()
        {
            var gameStatus = Start();
            gameStatus.UserName.Should().Be(_gameSettings.DefaultUserName);
        }

        [Fact]
        public void Start_ShouldReturnThreeShipsPerPlayer()
        {
            var gameStatus = Start();
            gameStatus.UserGrid.GetShips().Count.Should().Be(3);
            gameStatus.ComputerGrid.GetShips().Count.Should().Be(3);
        }

        [Fact]
        public void Start_ShouldReturnOneBattleshipAndTwoDestroyersForEachGrid()
        {
            var gameStatus = Start();
            gameStatus.UserGrid.GetShips().Where(n => n.Count() == 5).Count().Should().Be(1);
            gameStatus.UserGrid.GetShips().Where(n => n.Count() == 4).Count().Should().Be(2);
            gameStatus.ComputerGrid.GetShips().Where(n => n.Count() == 5).Count().Should().Be(1);
            gameStatus.ComputerGrid.GetShips().Where(n => n.Count() == 4).Count().Should().Be(2);
        }

        [Fact]
        public void Start_ShipCoordinatesShouldBeNextToEachOtherAndInOneDirection()
        {
            var gameStatus = Start();

            foreach (var ship in gameStatus.UserGrid.GetShips().Concat(gameStatus.ComputerGrid.GetShips()))
            {
                CoordinatesAreNextToEachOther(ship.Select(n => n.Key).ToList()).Should().BeTrue();
            }
        }

        [Fact]
        public void Start_ShipCoordinatesShouldBeInsideGrid()
        {
            var gameStatus = Start();

            foreach (var ship in gameStatus.UserGrid.GetShips().Concat(gameStatus.ComputerGrid.GetShips()))
            {
                foreach (var coordinates in ship.Keys)
                {
                    coordinates.Horizontal.Should().BeLessThan(_gameSettings.GridSize);
                    coordinates.Vertical.Should().BeLessThan(_gameSettings.GridSize);
                }
            }
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
                if (coordinates[i] + 1 != coordinates[i + 1])
                    return false;
            }

            return true;
        }
    }
}
