namespace Yo
{
    public class MetaTable : YoEngine
    {
        sys_table m_sysTable;
        public sys_table SysTable { get { return m_sysTable; } }

        public MetaTable(string table) : base(table) { }

        public bool GetTable() {
            var result = false;
            while (true) {
                m_sysTable = null;
                var sql = string.Format("SELECT {0}, {1} FROM information_schema.tables WHERE table_schema='{2}'",
                            table_name, table_comment, m_schema_table);
                sql += string.Format(" and `{0}`='{1}'", table_name, m_table);
                if (!FindRow(sql)) {
                    break;
                }

                m_sysTable = new sys_table();
                m_sysTable.table_name = m_table;

                // comment
                var comment = YoConvert.Text2Json(m_dataRow[table_comment] as string);
                if (comment != null) {
                    // title
                    var title = comment[nameof(m_sysTable.title)];
                    if (title != null) {
                        m_sysTable.title = title.ToString();
                    }
                }

                // get refer
                var refer = (new MetaRefer(m_table)).GetReferTables();
                m_sysTable.refer = string.Join(",", refer);

                result = true;
                break;
            }
            return result;
        }

        public bool DropTable() {
            return runSql("DROP TABLE " + m_table);
        }
    }
}
