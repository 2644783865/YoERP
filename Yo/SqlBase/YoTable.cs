using System.Collections.Generic;

namespace Yo
{
    public class YoTable : YoConnect
    {
        public Dictionary<string, yo_table> GetTables(object trans = null) {
            var dict = new Dictionary<string, yo_table>();

            var sql = string.Format("SELECT {0}, {1}, {2} FROM information_schema.tables WHERE table_schema='{3}';",
                yo_table.name, yo_table.rows, yo_table.comment, m_schema_table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_table>(m_dataTable, yo_table.name, row => {
                    return ConfigHelper.Translate(row[yo_table.name], trans);
                });
            }

            return dict;
        }

        public object GetComment(string table) {
            object result = null;
            var sql = string.Format("SELECT {0} FROM information_schema.tables WHERE table_schema='{1}' AND table_name='{2}';",
                yo_table.comment, m_schema_table, table);

            if (getData(sql)) {
                var rows = YoSqlHelper.GetRows(m_dataTable);
                if (rows != null) {
                    result = rows[0][yo_table.comment];
                }
            }

            return result;
        }

    }
}
