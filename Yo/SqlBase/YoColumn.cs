using System.Collections.Generic;

namespace Yo
{
    public class YoColumn : YoConnect
    {
        public Dictionary<string, yo_column> GetColumns(string table, object trans = null) {
            var dict = new Dictionary<string, yo_column>();
            if (string.IsNullOrEmpty(table)) {
                return dict;
            }

            var sql = string.Format("SELECT {0},{1},{2},{3} FROM information_schema.columns WHERE table_schema='{4}' AND table_name='{5}';",
                yo_column.name, yo_column.datatype, yo_column.datatypeEx, yo_column.comment, m_schema_table, table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_column>(m_dataTable, yo_column.name, row => {
                    return ConfigHelper.Translate(row[yo_column.name], trans);
                });
            }

            return dict;
        }

    }
}
