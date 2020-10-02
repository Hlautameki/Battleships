using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Battleships.Tests
{
    public class RandomCoordinatesGeneratorTests
    {
        IRandomCoordinatesGenerator _rcg;
        GameSettings _gameSettings = new GameSettings();


        public RandomCoordinatesGeneratorTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton<GameSettings>(_gameSettings);
            services.AddTransient<IRandomCoordinatesGenerator, RandomCoordinatesGenerator>();
            var serviceProvider = services.BuildServiceProvider();

            _rcg = serviceProvider.GetService<IRandomCoordinatesGenerator>();
        }

        [Fact]
        public void GetCoordinates_ReturnRandomCoordinatesFromDefiniedInGameSettingsRange()
        {
            for (int i = 0; i < 20; i++)
            {
                var coordinates = _rcg.GetCoordinates();

                coordinates.Horizontal.Should().BeGreaterOrEqualTo(0);
                coordinates.Horizontal.Should().BeLessThan(_gameSettings.GridSize);
                coordinates.Vertical.Should().BeGreaterOrEqualTo(0);
                coordinates.Vertical.Should().BeLessThan(_gameSettings.GridSize);
            }
        }
    }
}