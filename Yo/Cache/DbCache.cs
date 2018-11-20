namespace Yo
{
    public class DbCache : YoEngine, IYoCache
    {
        protected string m_table;

        public DbCache(string table) {
            m_table = table.EndsWith(CACHE) ? table : table + CACHE;
            LoadConfig(CACHE_TABLE);
        }

        public object sqlGet(int id) {
            var sql = string.Format("SELECT `{0}` FROM `{1}` WHERE id='{2}';", TITLE, m_table, id);
            return getFirst(sql);
        }

        public bool sqlSet(int id, object value) {
            var sql = string.Format("REPLACE INTO `{0}`(`{1}`, `{2}`) VALUES('{3}', '{4}');", m_table, ID, TITLE, id, value);
            return runSql(sql);
        }

        public bool sqlDelete(int id) {
            var sql = string.Format("DELETE FROM `{0}` WHERE `{1}`='{2}';", m_table, ID, id);
            return runSql(sql);
        }

        public object Get(object id) {
            if(!YoConvert.ToInt(ref id)) {
                return null;
            }
            return sqlGet((int)id);
        }

        public bool Set(object id, object value) {
            var result = false;
            while (true) {
                if (!YoConvert.ToInt(ref id)) {
                    break;
                }

                if(value == null) {
                    result = sqlDelete((int)id);
                    break;
                }

                result = sqlSet((int)id, value);
                break;
            }
            return result;
        }

    }
}
