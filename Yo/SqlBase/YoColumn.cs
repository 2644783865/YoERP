using System.Collections.Generic;

namespace Yo
{
    public class YoColumn : YoConnect
    {
        yo_column c_column;
        string c_name;
        string c_datatype;
        string c_datatypeEx;
        string c_comment;

        public YoColumn() {
            c_column = null;
            c_name = nameof(c_column.column_name);
            c_datatype = nameof(c_column.data_type);
            c_datatypeEx = nameof(c_column.column_type);
            c_comment = nameof(c_column.column_comment);
        }

        public Dictionary<string, yo_column> GetColumns(string table, object trans = null) {
            var dict = new Dictionary<string, yo_column>();
            if (string.IsNullOrEmpty(table)) {
                return dict;
            }

            var sql = string.Format("SELECT {0},{1},{2},{3} FROM information_schema.columns WHERE table_schema='{4}' AND table_name='{5}';",
                c_name, c_datatype, c_datatypeEx, c_comment, m_schema_table, table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_column>(m_dataTable, c_name, (key, row) => {
                    object obj = null;
                    switch (key) {
                        case nameof(c_column._display):
                            obj = ConfigHelper.Translate(row[c_name], trans);
                            break;
                        case nameof(c_column._commentobj):
                            obj = YoTool.GetJObject(row[c_comment].ToString());
                            break;
                    }
                    return obj;
                });
            }

            return dict;
        }

    }
}
