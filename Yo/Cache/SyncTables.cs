using MySql.Data.MySqlClient;

namespace Yo
{
    public class SyncTables : YoEngine
    {
        public SyncTables() {
            LoadConfig(CACHE_TABLE);
        }

        public void DeleteAll() {
            var objCacheTables = new YoTableInfo();
            objCacheTables.LoadConfig(CACHE_TABLE);
            var cacheTablesDict = objCacheTables.GetTableDict();

            foreach (var table in cacheTablesDict.Keys) {
                RunSql("truncate table " + table);
            }
        }

        public void Sync() {
            var objTables = new YoTableInfo();
            var tables = objTables.GetTableDict().Keys;

            var objCacheTables = new YoTableInfo();
            objCacheTables.LoadConfig(CACHE_TABLE);
            var cacheTablesDict = objCacheTables.GetTableDict();

            foreach(var table in tables) {
                var cacheTable = table + "_cache";
                if (!cacheTablesDict.ContainsKey(cacheTable)) {
                    CreateTable(cacheTable);
                }
            }
        }

        public bool CreateTable(string table) {
            var fields = "      `id` INT NOT NULL,  "
                        + "     `title` VARCHAR(31) NOT NULL,   "
                        + "     PRIMARY KEY(`id`)   ";
            var sql = string.Format("CREATE TABLE `{0}` ({1}) ENGINE = MEMORY", table, fields);
            return RunSql(sql);
        }

        public bool RunSql(string sql) {
            try {
                using (var conn = new MySqlConnection(m_connectionString)) {
                    conn.Open();
                    var cmd = new MySqlCommand(sql, conn);
                    var result = cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (MySqlException e) {
                m_errorDict[DB] = e.Message;
                return false;
            }
        }

    }
}
