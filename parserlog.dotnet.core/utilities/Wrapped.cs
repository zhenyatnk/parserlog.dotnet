namespace parserlog.dotnet.core.utilities
{
    public class Wrapped<T>
    {
        public Wrapped(T _value)
        {
            value = _value;
        }

        public T value { get; set; }
    }
}
