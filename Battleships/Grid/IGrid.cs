using System.Collections.Generic;

namespace Battleships
{
    public interface IGrid<T> where T : GridTypeResolver
    {
        IList<Dictionary<Coordinates, FieldType>> GetShips();

        Dictionary<Coordinates, FieldType> GetAllFields();
        
        void Initialize();

        void SetFieldType(Coordinates coordinates, FieldType fieldType);
    }
}