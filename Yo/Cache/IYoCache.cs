namespace Yo
{
    public interface IYoCache
    {
        object Get(object id);
        bool Set(object id, object value);
    }
}
