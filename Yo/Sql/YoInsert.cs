using System.Collections.Generic;

namespace Yo
{
    public class YoInsert : YoReplace
    {
        List<string> m_listSqlColumn;
        List<string> m_listSqlValue;

        public YoInsert(string table) : base(table) { }

        public bool Insert(Dictionary<string, object> dict) {
            var result = false;
            while (true) {
                if (dict == null) {
                    break;
                }

                if (dict.Count <= 0) {
                    break;
                }

                if (!parseSqlValues(dict)) {
                    break;
                }

                var sql = string.Format("INSERT INTO `{0}` ({1}) VALUES ({2});", m_table, string.Join(",", m_listSqlColumn), string.Join(",", m_listSqlValue));
                if (!runSql(sql)) {
                    break;
                }

                result = true;
                break;
            }

            return result;
        }

        bool parseSqlValues(Dictionary<string, object> uiDict) {
            bool result = false;
            while (true) {
                m_uiDict = uiDict;
                if (m_uiDict == null) {
                    break;
                }

                m_errorDict = new Dictionary<string, object>();
                m_listSqlColumn = new List<string>();
                m_listSqlValue = new List<string>();

                YoSqlHelper.EachColumn(m_yoColumnDict, (key, column) => {
                    while (true) {
                        object value = null;
                        if (!checkColumn(ref value, m_uiDict, key, column)) {
                            break;
                        }

                        m_listSqlColumn.Add(string.Format("`{0}`", key));
                        m_listSqlValue.Add(string.Format("'{0}'", value));
                        break;
                    }
                });

                if (m_errorDict.Count > 0) {
                    break;
                }

                if (m_listSqlValue.Count == 0) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

    }
}
