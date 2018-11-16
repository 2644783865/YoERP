using MySql.Data.MySqlClient;

namespace Yo
{
    public class DbCache : YoConnect, IYoCache
    {
        protected const string TITLE = "title";
        protected string m_table;

        public DbCache(string table) {
            m_table = table;
            LoadConfig(CACHE_TABLE);
        }

        public object sqlGet(int id) {
            object result = null;
            var sql = string.Format("SELECT `{0}` FROM `{1}` WHERE id='{2}';", TITLE, m_table, id);
            try {
                using (var conn = new MySqlConnection(m_connectionString)) {
                    conn.Open();
                    var cmd = new MySqlCommand(sql, conn);
                    result = cmd.ExecuteScalar();
                }
            }
            catch {
            }

            return result;
        }

        public bool sqlSet(int id, object value) {
            var result = false;
            var sql = string.Format("REPLACE INTO `{0}`(`{1}`, `{2}`) VALUES('{3}', '{4}');", m_table, ID, TITLE, id, value);

            try {
                using (var conn = new MySqlConnection(m_connectionString)) {
                    conn.Open();
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch {
            }

            return result;
        }

        public bool sqlDelete(int id) {
            var result = false;
            var sql = string.Format("DELETE FROM `{0}` WHERE `{1}`='{2}';", m_table, ID, id);

            try {
                using (var conn = new MySqlConnection(m_connectionString)) {
                    conn.Open();
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch {
            }

            return result;
        }

        public object Get(object id) {
            if(!YoTool.ParseInt(ref id)) {
                return null;
            }
            return sqlGet((int)id);
        }

        public bool Set(object id, object value) {
            var result = false;
            while (true) {
                if (!YoTool.ParseInt(ref id)) {
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
