using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSQL : YoConnect
    {
        protected const string ID = "id";
        protected const string ROW_TITLE = "title";
        protected const string FORMAT = "format";

        protected string m_table;
        public Dictionary<string, yo_column> ColumnDict;
        public Dictionary<string, object> ErrorList;

        public YoSQL(string table) {
            m_table = table;
            var obj = new YoColumn();
            ColumnDict = obj.GetColumns(m_table);
        }

        public bool ui2db(ref object value, yo_column column) {
            var result = true;
            switch (column.data_type) {
                case DataType.Number:
                    result = YoTool.ParseDouble(ref value);
                    break;
                case DataType.Identity:
                    result = YoTool.ParseInt(ref value);
                    break;
                case DataType.Text:
                    result = YoTool.ParseString(ref value);
                    break;
                case DataType.Datetime:
                    result = YoTool.ParseDatetime(ref value);
                    break;
            }
            return result;
        }

        public object db2ui(object value, yo_column column) {
            object result = value;
            switch (column.data_type) {
                case DataType.Number: {
                        var format = ConfigHelper.GetValue(column._commentobj, FORMAT);
                        if (format != null) {
                            var formatStr = "{0:F" + format + "}";
                            result = string.Format(formatStr, value);
                        }
                    }
                    break;
                case DataType.Datetime: {
                        var format = ConfigHelper.GetValue(column._commentobj, FORMAT);
                        if (format != null) {
                            result = Convert.ToDateTime(value).ToShortDateString();
                        }
                    }
                    break;
            }
            return result;
        }

    }
}
