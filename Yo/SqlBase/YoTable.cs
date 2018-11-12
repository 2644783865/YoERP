using System.Collections.Generic;

namespace Yo
{
    public class YoTable : YoConnect
    {
        public Dictionary<string, yo_table> GetTables() {
            var dict = new Dictionary<string, yo_table>();

            var sql = string.Format("SELECT {0}, {1}, {2} FROM information_schema.tables WHERE table_schema='{3}';",
                yo_table.name, yo_table.rows, yo_table.comment, m_schema_table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_table>(m_dataTable, yo_table.name);
            }

            return dict;
        }

    }
}
