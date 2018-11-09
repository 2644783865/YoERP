using System.Collections.Generic;

namespace Yo
{
    public class YoColumn : YoConnect
    {
        public Dictionary<string, yo_column> GetColumns(string table) {
            var dict = new Dictionary<string, yo_column>();
            if (string.IsNullOrEmpty(table)) {
                return dict;
            }

            var sql = string.Format("SELECT {0},{1},{2},{3} FROM information_schema.columns WHERE table_schema='{4}' AND table_name='{5}';",
                yo_column.name, yo_column.datatype, yo_column.datatypeEx, yo_column.comment, m_schema_table, table);

            fill(sql, ds => {
                dict = YoSqlHelper.Dataset2Dict<yo_column>(ds, yo_column.name);
            });

            return dict;
        }

    }
}
