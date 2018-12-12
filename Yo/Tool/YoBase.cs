using System.Collections.Generic;

namespace Yo
{
    public class YoBase
    {
        protected const string DB = nameof(DB);
        protected const string SYS = nameof(SYS);

        protected Dictionary<string, object> m_errorDict;
        public Dictionary<string, object> ErrorDict { get { return m_errorDict; } }

        public YoBase() {
            m_errorDict = new Dictionary<string, object>();
        }

    }
}
