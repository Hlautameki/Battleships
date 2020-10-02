namespace Battleships.ConsoleApp
{
    public static class FieldTypeExtensions
    {
        public static char ToChar(this FieldType field)
        {
            switch (field)
            {
                case FieldType.See:
                return '.';
                case FieldType.Hit:
                return 'Ø';
                case FieldType.Miss:
                return '*';
                case FieldType.Ship:
                return 'O';
                default:
                return '.';
            }
        }
    }
}