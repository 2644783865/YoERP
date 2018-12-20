using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yo
{
    public class YoInsert : YoReplace
    {
        List<string> m_listSqlColumn;
        List<string> m_listSqlValue;
        object m_id = null;
        public object Id { get { return m_id; } }

        public YoInsert(string table, object trans = null) : base(table, trans) { }

        public bool Insert(JObject uiDict) {
            var result = false;
            while (true) {
                m_id = null;
                m_uiDict = uiDict;
                if (m_uiDict == null) {
                    break;
                }

                if (m_uiDict.Count <= 0) {
                    break;
                }

                if (!parseSqlValues()) {
                    break;
                }

                var sql = string.Format("INSERT INTO `{0}` ({1}) VALUES ({2});", m_table, string.Join(",", m_listSqlColumn), string.Join(",", m_listSqlValue));
                sql += "SELECT last_insert_id();";
                m_id = getFirst(sql);
                if (m_id == null) {
                    break;
                }

                // create cache
                var selectone = new YoSelectOne(m_table, m_trans);
                selectone.GetRowTitleDisplay(m_id);

                result = true;
                break;
            }

            return result;
        }

        bool parseSqlValues() {
            bool result = false;
            while (true) {
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
