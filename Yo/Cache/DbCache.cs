namespace Yo
{
    public class DbCache : YoEngine, IYoCache
    {
        protected string m_tableCache;

        public DbCache(string table) {
            m_tableCache = table + CACHE;
            LoadConfig(CACHE_TABLE);
        }

        object sqlGet(int id) {
            var sql = string.Format("SELECT `{0}` FROM `{1}` WHERE id='{2}';", TITLE, m_tableCache, id);
            return getFirst(sql);
        }

        bool sqlSet(int id, object value) {
            var sql = string.Format("REPLACE INTO `{0}`(`{1}`, `{2}`) VALUES('{3}', '{4}');", m_tableCache, ID, TITLE, id, value);
            return runSql(sql);
        }

        bool sqlDelete(int id) {
            var sql = string.Format("DELETE FROM `{0}` WHERE `{1}`='{2}';", m_tableCache, ID, id);
            return runSql(sql);
        }

        public object Get(object id) {
            if(!YoConvert.ToInt(ref id)) {
                return null;
            }

            var result = sqlGet((int)id);
            if (result == null && m_errorDict.ContainsKey(DB)) {
                createTable();
            }
            return result;
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
            if (!result && m_errorDict.ContainsKey(DB)) {
                if (createTable()) {
                    result = Set(id, value);
                }
            }
            return result;
        }

        bool createTable() {
            var fields = "      `id` INT NOT NULL,  "
                        + "     `title` VARCHAR(31) NOT NULL,   "
                        + "     PRIMARY KEY(`id`)   ";
            var sql = string.Format("CREATE TABLE `{0}` ({1}) ENGINE = MEMORY", m_tableCache, fields);
            return runSql(sql);
        }
    }
}
