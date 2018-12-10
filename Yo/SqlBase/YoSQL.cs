using System.Collections.Generic;

namespace Yo
{
    public class YoSQL : YoEngine
    {
        protected const string UPTIME = "uptime";
        protected const string FORMAT = "format";

        protected string m_table;
        protected object m_trans;
        protected Dictionary<string, yo_column> m_yoColumnDict;
        protected List<string> m_titleFields;
        protected IYoCache m_cache;

        public Dictionary<string, yo_column> YoColumnDict { get { return m_yoColumnDict; } }

        public YoSQL(string table, object trans = null) {
            m_table = table;
            m_trans = trans;
            m_yoColumnDict = (new YoColumnInfo()).GetColumnDict(m_table, m_trans);
            m_titleFields = (new YoTableInfo()).GetTitleFields(m_table);
            m_cache = new DbCache(m_table);
        }

        public bool Find(object id) {
            var sql = string.Format("SELECT * FROM `{0}` where id='{1}'", m_table, id);
            return FindRow(sql);
        }

    }
}
