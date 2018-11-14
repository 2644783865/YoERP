using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yo
{
    public class YoTable : YoConnect
    {
        yo_table c_table;
        string c_name;
        string c_rows;
        string c_comment;

        public YoTable() {
            c_table = null;
            c_name = nameof(c_table.table_name);
            c_rows = nameof(c_table.table_rows);
            c_comment = nameof(c_table.table_comment);
        }

        public Dictionary<string, yo_table> GetTables(object trans = null) {
            var dict = new Dictionary<string, yo_table>();

            var sql = string.Format("SELECT {0}, {1}, {2} FROM information_schema.tables WHERE table_schema='{3}';",
                    c_name, c_rows, c_comment, m_schema_table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_table>(m_dataTable, c_name, (key, row) => {
                    object obj = null;
                    switch (key) {
                        case nameof(c_table._display):
                            obj = ConfigHelper.Translate(row[c_name], trans);
                            break;
                        case nameof(c_table._commentobj):
                            obj = YoTool.GetJObject(row[c_comment].ToString());
                            break;
                    }
                    return obj;
                });
            }

            return dict;
        }

        public JObject GetComment(string table) {
            JObject result = null;
            var sql = string.Format("SELECT {0} FROM information_schema.tables WHERE table_schema='{1}' AND table_name='{2}';",
                c_comment, m_schema_table, table);

            if (getData(sql)) {
                var rows = YoSqlHelper.GetRows(m_dataTable);
                if (rows != null) {
                    result = YoTool.GetJObject(rows[0][c_comment].ToString());
                }
            }

            return result;
        }

    }
}
