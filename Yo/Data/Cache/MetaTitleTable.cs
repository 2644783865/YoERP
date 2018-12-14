namespace Yo
{
    public class MetaTitleTable : YoEngine
    {
        string m_titleTable;
        public MetaTitleTable(string table) {
            m_titleTable = title_ + table;
            LoadConfig(CACHE_TABLE);
        }

        public bool HasTable() {
            var sql = string.Format("SELECT {0} FROM information_schema.tables WHERE table_schema='{1}'",
                        table_name, m_schema_table);
            sql += string.Format(" and `{0}`='{1}'", table_name, m_titleTable);
            return FindRow(sql);
        }
        
        public bool CreateTable() {
            var fields = "      `id` INT NOT NULL,  "
                        + "     `title` VARCHAR(31) NOT NULL,   "
                        + "     PRIMARY KEY(`id`)   ";
            var sql = string.Format("CREATE TABLE `{0}` ({1}) ENGINE = MyISAM", m_titleTable, fields);
            return runSql(sql);
        }

        public bool DropTable() {
            return runSql("DROP TABLE " + m_titleTable);
        }

        public bool CleanTable() {
            return runSql("truncate table " + m_titleTable);
        }

    }
}
