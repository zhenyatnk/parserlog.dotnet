using System;

namespace parserlog.dotnet.core.model
{
    public class LineInfo
    {
        public TimeSpan m_TimeStamp { get; set; }
        public UInt64 m_ThreadId { get; set; }
        public String m_Type { get; set; }
        public String m_Component { get; set; }
    }
}
