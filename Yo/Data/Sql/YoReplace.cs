using System.Collections.Generic;

namespace Yo
{
    public class YoReplace : YoSQL
    {
        protected Dictionary<string, object> m_uiDict;

        public Dictionary<string, object> UiDict { set { m_uiDict = value; } }

        public YoReplace(string table, object trans = null) : base(table, trans) { }

        bool ui2db(ref object value, sys_column yoColumn) {
            var result = true;
            switch (yoColumn.datatype) {
                case DataType.ID:
                case DataType.Refer:
                    result = YoConvert.ToInt(ref value);
                    break;
                case DataType.Number:
                case DataType.Calc:
                    result = YoConvert.ToDouble(ref value);
                    break;
                case DataType.Text:
                case DataType.Set:
                    result = YoConvert.ToString(ref value);
                    break;
                case DataType.Datetime:
                    result = YoConvert.ToDatetime(ref value);
                    break;
            }
            return result;
        }

        protected bool checkColumn(ref object value, Dictionary<string, object> uiDict, string key, sys_column yoColumn) {
            bool result = false;
            while (true) {
                if (key == ID) {
                    break;
                }

                if (key == UPTIME) {
                    break;
                }

                if (yoColumn.datatype == DataType.Calc) {
                    break;
                }

                if (!uiDict.ContainsKey(key)) {
                    break;
                }

                value = uiDict[key];
                if (!ui2db(ref value, yoColumn)) {
                    m_errorDict.Add(key, value);
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

    }
}
