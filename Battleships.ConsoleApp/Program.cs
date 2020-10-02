using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;
namespace Battleships.ConsoleApp
{
    class Program
    {
        static IServiceProvider _serviceProvider;

        static StringToCoordinatesConverter _converter = new StringToCoordinatesConverter();

        static void Main(string[] args)
        {
            try
            {
                RegisterServices();

                Run();
            }
            catch
            {
                WriteLine("We're sorry - We've run into an issue.");
            }
            finally
            {
                DisposeServices();
            }
        }

        public static void Run()
        {
            OutputEncoding = System.Text.Encoding.UTF8;

            const string userPromptMessage = "Specify a square to target: ";
            var game = _serviceProvider.GetService<Game>();
            var status = game.GetStatus();
            string input;
            var boardDrawer = new BoardDrawer();
            bool continueGame = true;
            string statusMessage = default;

            string[] exitWords = new string[] {"quit", "exit"};

            do
            {
                boardDrawer.DrawBoard(status, userPromptMessage, statusMessage);
                statusMessage = default;
                input = ReadLine();
                if (exitWords.Contains(input.ToLower().Trim()) || exitWords.Contains(input.ToLower().Trim()))
                    continueGame = false;
                else
                {
                    try
                    {
                        var coordinates = _converter.Convert(input);
                        status = game.Shoot(coordinates);
                        if (!string.IsNullOrEmpty(game.GetWinner()))
                            break;
                        status = game.Shoot();
                        if (!string.IsNullOrEmpty(game.GetWinner()))
                            break;
                    }
                    catch (System.ArgumentException ex)
                    {
                        statusMessage = ex.Message;
                    }
                }

            } while (continueGame);
            
            if (!string.IsNullOrEmpty(game.GetWinner()))
                boardDrawer.DrawBoard(status, $"Game Over! The winner is: {game.GetWinner()}");

            WriteLine();
            WriteLine();
            ReadKey();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<GameSettings>();
            services.AddTransient<IShipBuilder, ShipBuilder>();
            services.AddTransient<Game>();
            services.AddTransient<IGrid<UserGridType>, Grid<UserGridType>>();
            services.AddTransient<IGrid<ComputerGridType>, ComputerGrid<ComputerGridType>>();
            services.AddTransient<GameStatus>();
            services.AddTransient<AI>();
            services.AddTransient<IRandomCoordinatesGenerator, RandomCoordinatesGenerator>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
