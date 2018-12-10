using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSelectOne : YoSQL
    {
        public YoSelectOne(string table, object trans = null) : base(table, trans) { }

        object db2ui(object value, yo_column yoColumn) {
            object result = value;
            switch (yoColumn._datatype) {
                case DataType.Number:
                case DataType.Calc: {
                        var format = ConfigHelper.GetValue(yoColumn._comment, FORMAT);
                        if (format != null) {
                            var formatStr = "{0:F" + format + "}";
                            result = string.Format(formatStr, value);
                        }
                    }
                    break;
                case DataType.Datetime: {
                        var format = ConfigHelper.GetValue(yoColumn._comment, FORMAT);
                        if (format != null) {
                            result = Convert.ToDateTime(value).ToShortDateString();
                        }
                    }
                    break;
                case DataType.Set:
                    if (value != null && yoColumn._info is Dictionary<string, string>) {
                        var setKey = value.ToString();
                        var setDict = yoColumn._info as Dictionary<string, string>;
                        if (setDict.ContainsKey(setKey)) {
                            result = setDict[setKey];
                        }
                    }
                    break;
            }
            return result;
        }

        protected string getColumnDisplay(object value, yo_column yoColumn) {
            string result = null;
            while (true) {
                if (yoColumn._datatype == DataType.Refer) {
                    var select = new YoSelect(yoColumn._info as string);
                    result = select.getRowTitleDisplay(value);
                    break;
                }

                var obj = db2ui(value, yoColumn);
                if (obj == null) {
                    break;
                }

                result = obj.ToString();
                break;
            }
            return result;
        }

        protected string getRowTitleDisplay(object id, bool isRow = false) {
            string result = null;
            while (true) {
                // get cache
                var temp = m_cache.Get(id);
                if (temp != null) {
                    return temp.ToString();
                }

                DataRow row = null;
                if (isRow) {
                    row = id as DataRow;
                }
                else if(Find(id)) {
                    row = m_dataRow;
                }

                if (row == null) {
                    break;
                }

                var titleList = new List<string>();
                foreach (var field in m_titleFields) {
                    if (!m_yoColumnDict.ContainsKey(field)) {
                        continue;
                    }

                    var yoColumn = m_yoColumnDict[field];
                    var title = getColumnDisplay(row[field], yoColumn);
                    titleList.Add(title);
                }

                result = string.Join("_", titleList);
                break;
            }

            // set cache
            if (result != null) {
                m_cache.Set(id, result);
            }

            return result;
        }

        public Dictionary<string, yo_column_ui> GetUIColumnDict(bool isModify = true) {
            var uiColumnDict = new Dictionary<string, yo_column_ui>();
            if (isModify && m_dataRow == null) {
                return uiColumnDict;
            }

            foreach(var yoColumn in m_yoColumnDict.Values) {
                var ui_column = new yo_column_ui();
                ui_column.column = yoColumn;

                var key = yoColumn.column_name;
                if (isModify) {
                    var value = m_dataRow[key];
                    var displayValue = getColumnDisplay(value, yoColumn);
                    ui_column.column_value = new yo_column_value(value, displayValue);
                }
                uiColumnDict.Add(key, ui_column);
            }
            return uiColumnDict;
        }

    }
}
