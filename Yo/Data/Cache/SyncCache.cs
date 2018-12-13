namespace Yo
{
    public class SyncCache : YoEngine
    {
        public SyncCache() {
            LoadConfig(CACHE_TABLE);
        }

        public void CleanTables() {
            var objCacheTables = new YoTableInfo();
            objCacheTables.LoadConfig(CACHE_TABLE);
            var cacheTablesDict = objCacheTables.GetTableDict();

            foreach (var table in cacheTablesDict.Keys) {
                cleanTable(table);
            }
        }

        void cleanTable(string table) {
            runSql("truncate table " + table);
        }

        public void SyncTables() {
            var objTables = new YoTableInfo();
            var tables = objTables.GetTableDict().Keys;

            var objCacheTables = new YoTableInfo();
            objCacheTables.LoadConfig(CACHE_TABLE);
            var cacheTablesDict = objCacheTables.GetTableDict();

            foreach(var table in tables) {
                var cacheTable = table + CACHE;
                if (!cacheTablesDict.ContainsKey(cacheTable)) {
                    createTable(cacheTable);
                }
            }
        }

        bool createTable(string table) {
            var fields = "      `id` INT NOT NULL,  "
                        + "     `title` VARCHAR(31) NOT NULL,   "
                        + "     PRIMARY KEY(`id`)   ";
            var sql = string.Format("CREATE TABLE `{0}` ({1}) ENGINE = MEMORY", table, fields);
            return runSql(sql);
        }

    }
}
