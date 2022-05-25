using ToBeApi.Models;
using System.Dynamic;

namespace ToBeApi.Data.Shaper
{
    public interface IDataShaper<T>
    {
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string
        fieldsString);
        ShapedEntity ShapeData(T entity, string fieldsString);
    }

}
