using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSQL : YoConnect
    {
        protected const string UPTIME = "uptime";
        protected const string ROW_TITLE = "title";
        protected const string FORMAT = "format";

        protected YoCache m_cache;
        protected string m_table;
        public Dictionary<string, yo_column> ColumnDict;
        public Dictionary<string, object> ErrorList;

        public YoSQL(string table) {
            m_table = table;
            var obj = new YoColumn();
            ColumnDict = obj.GetColumns(m_table);
            m_cache = new YoCache(m_table);
        }

        public bool checkColumn(ref object value, Dictionary<string, object> dict, string key, yo_column column) {
            bool result = false;
            while (true) {
                if (key == ID) {
                    break;
                }

                if (key == UPTIME) {
                    break;
                }

                if (column._datatype == DataType.Calc) {
                    break;
                }

                if (!dict.ContainsKey(key)) {
                    break;
                }

                value = dict[key];
                if (!ui2db(ref value, column)) {
                    ErrorList.Add(key, value);
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        public bool ui2db(ref object value, yo_column column) {
            var result = true;
            switch (column._datatype) {
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
            switch (column._datatype) {
                case DataType.Number:
                case DataType.Calc: {
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
                case DataType.Set:
                    if (value != null && column._set != null) {
                        var setKey = value.ToString();
                        if (column._set.ContainsKey(setKey)) {
                            result = column._set[setKey];
                        }
                    }
                    break;
            }
            return result;
        }

    }
}
