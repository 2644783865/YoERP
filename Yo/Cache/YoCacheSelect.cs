namespace Yo
{
    public class YoCacheSelect : YoEngine
    {
        string m_tableCache;
        int m_limit;

        public YoCacheSelect(string table, int limit = 10) {
            m_tableCache = table + CACHE;
            m_limit = limit;
            LoadConfig(CACHE_TABLE);
            (new YoCacheManage(table)).InitData();
        }

        public bool Select() {
            var sql = string.Format("SELECT * FROM `{0}` limit {1};", m_tableCache, m_limit);
            return fillData(sql);
        }

        public bool Select(string key) {
            var sql = string.Format("SELECT * FROM `{0}` WHERE `title` LIKE '%{1}%' limit {2};", m_tableCache, key, m_limit);
            return fillData(sql);
        }

    }
}
