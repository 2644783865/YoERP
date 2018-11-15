using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yo
{
    public class YoTable : YoConnect
    {
        public Dictionary<string, yo_table> GetTables(object trans = null) {
            var dict = new Dictionary<string, yo_table>();

            yo_table c;
            var sql = string.Format("SELECT {0}, {1}, {2} FROM information_schema.tables WHERE table_schema='{3}';",
                    nameof(c.table_name), nameof(c.table_rows), nameof(c.table_comment), m_schema_table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_table>(m_dataTable, nameof(c.table_name), (key, yoTable) => {
                    object obj = null;
                    switch (key) {
                        case nameof(c._display):
                            obj = ConfigHelper.Translate(yoTable.table_name, trans);
                            break;
                        case nameof(c._commentobj):
                            obj = YoTool.GetJObject(yoTable.table_comment);
                            break;
                    }
                    return obj;
                });
            }

            return dict;
        }

        public JObject GetComment(string table) {
            JObject result = null;

            yo_table c;
            var sql = string.Format("SELECT {0} FROM information_schema.tables WHERE table_schema='{1}' AND table_name='{2}';",
                nameof(c.table_comment), m_schema_table, table);

            if (getData(sql)) {
                var rows = YoSqlHelper.GetRows(m_dataTable);
                if (rows != null) {
                    result = YoTool.GetJObject(rows[0][nameof(c.table_comment)].ToString());
                }
            }

            return result;
        }

    }
}
