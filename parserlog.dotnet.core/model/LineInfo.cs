using System;

namespace parserlog.dotnet.core.model
{
    public class LineInfo
    {
        public LineInfo()
        {
            m_TimeStamp = new TimeSpan();
            m_ThreadId = 0;
            m_Type = "";
            m_Component = "";
        }
        public TimeSpan m_TimeStamp { get; set; }
        public UInt64 m_ThreadId { get; set; }
        public String m_Type { get; set; }
        public String m_Component { get; set; }
    }
}
