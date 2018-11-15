using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSqlHelper
    {
        static public DataRowCollection GetRows(DataTable dt) {
            DataRowCollection rows = null;
            while (true) {
                if (dt == null) {
                    break;
                }

                var temp = dt.Rows;
                if (temp.Count == 0) {
                    break;
                }

                rows = temp;
                break;
            }
            return rows;
        }

        static public DataView GetView(DataTable dt) {
            DataView view = null;
            while (true) {
                if (dt == null) {
                    break;
                }

                var temp = dt.DefaultView;
                if (temp.Count == 0) {
                    break;
                }

                view = temp;
                break;
            }
            return view;
        }

        static public void EachRow(DataRowCollection rows, Action<DataRow> act) {
            if (rows == null || act == null) {
                return;
            }

            foreach (DataRow row in rows) {
                act(row);
            }
        }

        static public void EachColumn(Dictionary<string, yo_column> columnDict, Action<string, yo_column> act) {
            if (columnDict == null || act == null) {
                return;
            }

            foreach (var item in columnDict) {
                act(item.Key, item.Value);
            }
        }

        static public Dictionary<string, T> Datatable2Dict<T>(DataTable dt, string key, Func<string, T, object> func = null, string display = "display") {
            var dict = new Dictionary<string, T>();
            while (true) {
                var rows = GetRows(dt);
                if (rows == null) {
                    break;
                }

                var type = typeof(T);
                var keyProperty = type.GetProperty(key);
                if (keyProperty == null || keyProperty.PropertyType != typeof(string)) {
                    break;
                }

                var propertyList = type.GetProperties();
                foreach (DataRow row in rows) {
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

        static public bool IsRelation(string key) {
            return key.EndsWith("_id");
        }

        static public string GetRelationTable(string key) {
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

        static public DataType GetDataType(string content, object commentobj) {
            if(ConfigHelper.GetValue(commentobj, "calc") != null) {
                return DataType.Calc;
            }

            DataType result = DataType.Unknown;
            switch (content) {
                case "double":
                    result = DataType.Number;
                    break;
                case "varchar":
                    result = DataType.Text;
                    break;
                case "int":
                    result = DataType.Identity;
                    break;
                case "datetime":
                case "timestamp":
                    result = DataType.Datetime;
                    break;
                case "set":
                    result = DataType.Set;
                    break;
            }
            return result;
        }

    }
}
