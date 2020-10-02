namespace Battleships
{
    public struct Coordinates
    {
        public int Vertical { get; set; }
        public int Horizontal { get; set; }

        public Coordinates(int horizontal, int vertical)
        {
            Vertical = vertical;
            Horizontal = horizontal;
        }
    }
}