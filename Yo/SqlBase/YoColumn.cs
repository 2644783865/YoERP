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

            yo_column c;
            var sql = string.Format("SELECT {0},{1},{2},{3} FROM information_schema.columns WHERE table_schema='{4}' AND table_name='{5}';",
                nameof(c.column_name), nameof(c.data_type), nameof(c.column_type), nameof(c.column_comment), m_schema_table, table);

            if (getData(sql)) {
                dict = YoSqlHelper.Datatable2Dict<yo_column>(m_dataTable, nameof(c.column_name), (key, yoColumn) => {
                    object obj = null;
                    switch (key) {
                        case nameof(c._display):
                            obj = ConfigHelper.Translate(yoColumn.column_name, trans);
                            break;
                        case nameof(c._commentobj):
                            obj = YoTool.GetJObject(yoColumn.column_comment);
                            break;
                        case nameof(c._datatype):
                            obj = YoSqlHelper.GetDataType(yoColumn.data_type, yoColumn._commentobj);
                            break;
                        case nameof(c._set):
                            if(yoColumn._datatype == DataType.Set) {
                                var setDict = new Dictionary<string, string>();
                                var itemArr = YoSqlHelper.GetSetList(yoColumn.column_type);
                                foreach(var setKey in itemArr) {
                                    var setValue = ConfigHelper.Translate(setKey, trans);
                                    setDict.Add(setKey, setValue);
                                }
                                obj = setDict;
                            }
                            break;
                    }
                    return obj;
                });
            }

            return dict;
        }

    }
}
