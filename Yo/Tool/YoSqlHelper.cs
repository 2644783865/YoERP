using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSqlHelper
    {
        static public DataRowCollection GetRows(DataSet ds) {
            DataRowCollection rows = null;
            while (true) {
                if (ds == null) {
                    break;
                }

                if (ds.Tables.Count == 0) {
                    break;
                }

                var temp = ds.Tables[0].Rows;
                if (temp.Count == 0) {
                    break;
                }

                rows = temp;
                break;
            }
            return rows;
        }

        static public DataView GetView(DataSet ds) {
            DataView view = null;
            while (true) {
                if (ds == null) {
                    break;
                }

                if (ds.Tables.Count == 0) {
                    break;
                }

                var temp = ds.Tables[0].DefaultView;
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

        static public Dictionary<string, T> Dataset2Dict<T>(DataSet ds, string key) {
            var dict = new Dictionary<string, T>();
            while (true) {
                var rows = GetRows(ds);
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
                        property.SetValue(item, row[name]);
                        if (name == key) {
                            dict.Add(row[name].ToString(), item);
                        }
                    }
                }
                break;
            }
            return dict;
        }

    }
}
