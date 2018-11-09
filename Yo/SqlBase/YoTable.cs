using System.Collections.Generic;

namespace Yo
{
    public class YoTable : YoConnect
    {
        public Dictionary<string, yo_table> GetTables() {
            var dict = new Dictionary<string, yo_table>();

            var sql = string.Format("SELECT {0}, {1}, {2} FROM information_schema.tables WHERE table_schema='{3}';",
                yo_table.name, yo_table.rows, yo_table.comment, m_schema_table);

            fill(sql, ds => {
                dict = YoSqlHelper.Dataset2Dict<yo_table>(ds, yo_table.name);
            });

            return dict;
        }

    }
}
