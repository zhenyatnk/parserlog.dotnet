using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parserlog.dotnet.model.utilities
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
