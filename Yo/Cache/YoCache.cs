using System.Data;

namespace Yo
{
    public class YoCache : YoConnect, IYoCache
    {
        protected string m_table;
        DbCache m_dbCache;

        public YoCache(string table) {
            m_table = table;
            m_dbCache = new DbCache(table + "_cache");
        }

        public void CacheTables(object id) {
            var TABLENAME = "table_name";
            var sql = string.Format("SELECT {0} FROM information_schema.referential_constraints WHERE constraint_schema='{1}' and referenced_table_name='{2}';",
                                TABLENAME, m_schema_table, m_table);
            if (!getData(sql)) {
                return;
            }

            var key = m_table + "_" + ID;
            foreach (DataRow row in m_dataTable.Rows) {
                var table = row[TABLENAME].ToString();
                var refer = new YoCache(table);
                refer.CacheRows(key, id);
            }
        }

        public void CacheRows(object key, object value) {
            var sql = string.Format("SELECT {0} FROM `{1}` WHERE `{2}`='{3}'", ID, m_table, key, value);
            if (!getData(sql)) {
                return;
            }

            foreach (DataRow row in m_dataTable.Rows) {
                CacheRow(row[ID]);
            }
        }

        public void CacheRow(object id) {
            if (Set(id, null)) {
                CacheTables(id);
            }
        }

        public object Get(object id) {
            return ((IYoCache)m_dbCache).Get(id);
        }

        public bool Set(object id, object value) {
            return ((IYoCache)m_dbCache).Set(id, value);
        }
    }
}
