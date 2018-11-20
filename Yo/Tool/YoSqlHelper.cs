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

        static public void EachColumn(Dictionary<string, yo_column> columnDict, Action<string, yo_column> act) {
            if (act == null || columnDict == null || columnDict.Count == 0) {
                return;
            }

            foreach (var item in columnDict) {
                act(item.Key, item.Value);
            }
        }

        static public Dictionary<string, T> Datatable2Dict<T>(DataTable dt, string key, Func<string, T, object> func = null) {
            var dict = new Dictionary<string, T>();
            while (true) {
                if(dt == null || dt.Rows.Count == 0) {
                    break;
                }

                var type = typeof(T);
                var keyProperty = type.GetProperty(key);
                if (keyProperty == null || keyProperty.PropertyType != typeof(string)) {
                    break;
                }

                var propertyList = type.GetProperties();
                foreach (DataRow row in dt.Rows) {
                    var item = (T)Activator.CreateInstance(type, null);

                    foreach (var property in propertyList) {
                        var name = property.Name;
                        object value = null;

                        if (name.StartsWith("_")) {
                            if (func == null) {
                                continue;
                            }
                            value = func(name, item);
                        }
                        else {
                            value = row[name];
                        }
                        property.SetValue(item, value);

                        if (name == key) {
                            dict.Add(row[name].ToString(), item);
                        }
                    }
                }
                break;
            }
            return dict;
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
