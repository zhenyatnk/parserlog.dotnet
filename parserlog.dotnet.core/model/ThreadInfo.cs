using System;
using System.Collections.Generic;

namespace parserlog.dotnet.core.model
{
    public class ThreadInfo
    {
        public ThreadInfo()
        {
            m_Id = 0;
            m_TimeStart = new TimeSpan();
            m_TimeEnd = new TimeSpan();
            m_Count = 0;
            m_Component = new SortedList<string, ulong>();
        }

        public UInt64 m_Id { get; set; }
        public TimeSpan m_TimeStart { get; set; }
        public TimeSpan m_TimeEnd { get; set; }
        public UInt64 m_Count { get; set; }
        public SortedList<String, UInt64> m_Component { get; set; }
    }
}
