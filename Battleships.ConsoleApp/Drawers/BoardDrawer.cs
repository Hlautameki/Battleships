using System;
using System.Linq;
using static System.Console;

namespace Battleships.ConsoleApp
{
    internal class BoardDrawer
    {
        GridDrawer gridDrawer = new GridDrawer();
        
        int _computerPlayerLeftShift = WindowWidth / 2;

        GameStatus _status;

        public void DrawBoard(GameStatus status, string promptMessage, string statusMessage = default)
        {
            _status = status;

            Clear();

            DrawGameName();

            WriteLine();

            DrawPlayerNames();

            WriteLine();

            DrawShipsStatuses();

            WriteLine();

            DrawGrids();

            WriteLine();

            WriteLine(statusMessage);

            Write(promptMessage);
        }

        private void DrawGameName()
        {
            WriteLine(String.Format("{0," + (_computerPlayerLeftShift + (_status.GameName.Length / 2)) + "}", _status.GameName));
        }

        private void DrawPlayerNames()
        {
            Write($"{_status.UserName}");
            WriteWithShift(_status.ComputerPlayerName);
        }

        private void DrawShipsStatuses()
        {
            var userShipsPosition = new Position(CursorTop, CursorLeft);
            var pcPlayerShipsPosition = new Position(CursorTop, _computerPlayerLeftShift);
            var shipsStatusDrawer = new ShipsStatusDrawer();

            shipsStatusDrawer.Draw(userShipsPosition, _status.UserGrid.GetShips());
            shipsStatusDrawer.Draw(pcPlayerShipsPosition, _status.ComputerGrid.GetShips());
        }

        private void DrawGrids()
        {
            int gridStartLine = CursorTop;
            var userGridPosition = new Position(gridStartLine, CursorLeft);
            var computerPlayerGridPosition = new Position(gridStartLine, _computerPlayerLeftShift);

            gridDrawer.DrawGrid(userGridPosition, _status.UserGrid.GetAllFields());
            gridDrawer.DrawGrid(computerPlayerGridPosition, _status.ComputerGrid.GetAllFields()
                .Where(n=>n.Value != FieldType.Ship).ToDictionary(k=>k.Key, k=>k.Value));
        }

        private void WriteWithShift(string text)
        {
            CursorLeft = _computerPlayerLeftShift;
            WriteLine(text);
            CursorLeft = 0;
        }
    }
}