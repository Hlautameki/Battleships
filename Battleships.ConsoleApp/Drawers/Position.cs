namespace Battleships.ConsoleApp
{        
    public struct Position
    {
        public int Top { get; private set; }
        public int Left { get; private set; }

        public Position(int top, int left)
        {
            Top = top;
            Left = left;
        }
    }
}
