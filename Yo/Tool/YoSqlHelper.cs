using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSqlHelper
    {
        static public void EachRow(DataRowCollection rows, Action<DataRow> act) {
            if (act == null || rows == null || rows.Count == 0) {
                return;
            }

            foreach (DataRow row in rows) {
                act(row);
            }
        }

        static public void EachColumn<T>(Dictionary<string, T> columnDict, Action<string, T> act) {
            if (act == null || columnDict == null || columnDict.Count == 0) {
                return;
            }

            foreach (var item in columnDict) {
                act(item.Key, item.Value);
            }
        }

        static public string GetReferTable(string key) {
            return key.Replace("_id", "");
        }

        static public List<string> GetSetList(object content) {
            var result = new List<string>();
            if(content == null) {
                return result;
            }

            var datatypeEx = content.ToString();
            var itemStr = datatypeEx.Replace("set(", "").Replace(")", "");
            var itemArr = itemStr.Split(',');
            foreach (var item in itemArr) {
                result.Add(item.Trim().Replace("'", ""));
            }
            return result;
        }

        static public DataType GetDataType(string key, string content, object commentobj) {
            DataType result = DataType.Unknown;
            if (key == "id") {
                result = DataType.ID;
            }
            else if (key.EndsWith("_id")) {
                result = DataType.Refer;
            }
            else {
                switch (content) {
                    case "int":
                    case "double":
                        result = ConfigHelper.GetValue(commentobj, "calc") == null ? DataType.Number : DataType.Calc;
                        break;
                    case "varchar":
                        result = DataType.Text;
                        break;
                    case "datetime":
                    case "timestamp":
                        result = DataType.Datetime;
                        break;
                    case "set":
                        result = DataType.Set;
                        break;
                }
            }
            return result;
        }

    }
}
