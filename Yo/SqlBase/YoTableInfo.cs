using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yo
{
    public class YoTableInfo : YoEngine
    {
        public Dictionary<string, yo_table> GetTableDict(object trans = null) {
            var dict = new Dictionary<string, yo_table>();

            yo_table c;
            var sql = string.Format("SELECT {0}, {1} FROM information_schema.tables WHERE table_schema='{2}'",
                    nameof(c.table_name), nameof(c.table_comment), m_schema_table);

            if (fillData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_table>(m_dataTable, nameof(c.table_name), (key, yoTable) => {
                    object obj = null;
                    switch (key) {
                        case nameof(c._translation):
                            obj = ConfigHelper.Translate(yoTable.table_name, trans);
                            break;
                        case nameof(c._comment):
                            obj = YoConvert.Text2Json(yoTable.table_comment);
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
            var sql = string.Format("SELECT {0} FROM information_schema.tables WHERE table_schema='{1}' AND table_name='{2}'",
                nameof(c.table_comment), m_schema_table, table);

            if(FindRow(sql)) {
                result = YoConvert.Text2Json(m_dataRow[nameof(c.table_comment)] as string);
            }
            return result;
        }

        public List<string> GetTitleFields(string table) {
            var result = new List<string>();
            while (true) {
                if (string.IsNullOrEmpty(table)) {
                    break;
                }

                var comment = GetComment(table);
                if(comment == null) {
                    break;
                }

                var fieldsObject = ConfigHelper.GetValue(comment, "title");
                if(fieldsObject == null) {
                    break;
                }

                var fields = fieldsObject.ToString();
                if (string.IsNullOrEmpty(fields)) {
                    break;
                }

                var fieldList = fields.Split(',');
                foreach (var field in fieldList) {
                    result.Add(field);
                }

                break;
            }

            if(result.Count == 0) {
                result.Add(ID);
            }

            return result;
        }

    }
}
