using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Battleships.Tests
{
    public class GameTests
    {
        private Game _game;

        private IGrid<UserGridType> _userGrid = Substitute.For<IGrid<UserGridType>>();
        private IGrid<ComputerGridType> _computerGrid = Substitute.For<IGrid<ComputerGridType>>();

        public GameTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<GameSettings>();
            services.AddTransient<IShipBuilder, ShipBuilder>();
            services.AddTransient<Game>();
            services.AddSingleton<IGrid<ComputerGridType>>(_computerGrid);
            services.AddSingleton<IGrid<UserGridType>>(_userGrid);
            services.AddTransient<GameStatus>();
            services.AddTransient<AI>();
            services.AddTransient<IRandomCoordinatesGenerator, RandomCoordinatesGenerator>();
            var serviceProvider = services.BuildServiceProvider();

            _game = serviceProvider.GetService<Game>();
        }

        [Theory]
        [InlineData(11, 10)]
        [InlineData(8, 12)]
        [InlineData(1, 10)]
        public void Shoot_InvalidCoordinates_ThrowArgumentException(int horizontal, int vertical)
        {
            _game.Invoking(n => n.Shoot(new Coordinates(horizontal, vertical)))
                .Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(5, 9)]
        [InlineData(8, 7)]
        public void Shoot_ValidCoordinates_ShouldNotThrowArgumentException(int horizontal, int vertical)
        {
            _game.Invoking(n => n.Shoot(new Coordinates(horizontal, vertical)))
                .Should().NotThrow<ArgumentException>();
        }

        [Theory]
        [InlineData(FieldType.Ship, FieldType.Hit)]
        [InlineData(FieldType.See, FieldType.Miss)]
        public void Shoot_ShouldUpdateFieldType(FieldType previousType, FieldType newType)
        {
            var coordinates = new Coordinates(5, 5);
            _computerGrid.GetAllFields().Returns(new Dictionary<Coordinates, FieldType>() { { coordinates, previousType } });

            var newGameStatus = _game.Shoot(coordinates);

            _computerGrid.Received().SetFieldType(coordinates, newType);
        }

        [Fact]
        public void Shoot_SecondTimeTheSameCoordinates_ShouldThrowException()
        {
            var coordinates = new Coordinates(5, 5);
            _computerGrid.GetAllFields().Returns(
                new Dictionary<Coordinates, FieldType>() { { coordinates, FieldType.See } },
                new Dictionary<Coordinates, FieldType>() { { coordinates, FieldType.Miss } }
                );

            var newGameStatus = _game.Shoot(coordinates);

            _game.Invoking(n => n.Shoot(coordinates))
                .Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<Dictionary<Coordinates, FieldType>>()
                {
                    {new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(1,1), FieldType.Hit },
                        { new Coordinates(1,2), FieldType.Hit },
                        { new Coordinates(1,3), FieldType.Hit }
                    }},
                    { new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(5,3), FieldType.Hit },
                        { new Coordinates(6,3), FieldType.Hit },
                        { new Coordinates(7,3), FieldType.Hit }
                    }}
                },
                new List<Dictionary<Coordinates, FieldType>>(),
                new GameSettings().DefaultUserName
            };
            yield return new object[]
            {
                new List<Dictionary<Coordinates, FieldType>>(),
                new List<Dictionary<Coordinates, FieldType>>()
                {
                    {new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(1,1), FieldType.Hit },
                        { new Coordinates(1,2), FieldType.Hit },
                        { new Coordinates(1,3), FieldType.Hit }
                    }},
                    { new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(5,3), FieldType.Hit },
                        { new Coordinates(6,3), FieldType.Hit },
                        { new Coordinates(7,3), FieldType.Hit }
                    }}
                },
                new GameSettings().DefaultComputerPlayerName
            };
            yield return new object[]
            {
                new List<Dictionary<Coordinates, FieldType>>()
                {
                    {new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(1,1), FieldType.Hit },
                        { new Coordinates(1,2), FieldType.Miss },
                        { new Coordinates(1,3), FieldType.Hit }
                    }},
                    { new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(5,3), FieldType.Hit },
                        { new Coordinates(6,3), FieldType.Hit },
                        { new Coordinates(7,3), FieldType.Hit }
                    }}
                },
                new List<Dictionary<Coordinates, FieldType>>()
                {
                    {new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(1,1), FieldType.Hit },
                        { new Coordinates(1,2), FieldType.Miss },
                        { new Coordinates(1,3), FieldType.Hit }
                    }},
                    { new Dictionary<Coordinates, FieldType>(){
                        { new Coordinates(5,3), FieldType.Hit },
                        { new Coordinates(6,3), FieldType.Hit },
                        { new Coordinates(7,3), FieldType.Hit }
                    }}
                },
                default
            };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void GetWinner_CheckWhoSanksAllEnemyShips_ReturnsWinnerName(
            List<Dictionary<Coordinates, FieldType>> computerShips,
            List<Dictionary<Coordinates, FieldType>> userShips,
            string winner)
        {
            _computerGrid.GetShips().Returns(computerShips);
            _userGrid.GetShips().Returns(userShips);

            _game.GetWinner().Should().Be(winner);
        }
    }
}