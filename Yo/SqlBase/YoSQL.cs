using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSQL : YoConnect
    {
        protected const string ID = "id";
        protected const string table_display = "display";

        protected string m_table;
        public Dictionary<string, yo_column> ColumnDict;
        public Dictionary<string, object> ErrorList;

        public YoSQL(string table) {
            m_table = table;
            var obj = new YoColumn();
            ColumnDict = obj.GetColumns(m_table);
        }

        public bool ui2db(string key, ref object value, string dataType) {
            var result = true;
            switch (dataType) {
                case DataType.Number:
                    result = YoTool.ParseDouble(ref value);
                    break;
                case DataType.Identity:
                    result = YoTool.ParseInt(ref value);
                    break;
                case DataType.Text:
                    result = YoTool.ParseString(ref value);
                    break;
            }
            return result;
        }

    }
}
