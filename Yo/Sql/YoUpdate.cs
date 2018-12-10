using System;
using System.Collections.Generic;

namespace Yo
{
    public class YoUpdate : YoReplace
    {
        Dictionary<string, string> m_sqlSetDict;

        public YoUpdate(string table, object trans = null) : base(table, trans) { }

        public bool Update(Dictionary<string, object> uiDict) {
            var result = false;
            while (true) {
                m_uiDict = uiDict;
                if (m_uiDict == null) {
                    break;
                }

                if (!m_uiDict.ContainsKey(ID)) {
                    break;
                }

                if (!Find(m_uiDict[ID])) {
                    break;
                }

                if (!checkUptime()) {
                    m_errorDict[DB] = "dirty data";
                    break;
                }

                if (!parseSqlSet()) {
                    break;
                }

                var sql = string.Format("UPDATE `{0}` SET {1} WHERE `id`='{2}';", m_table, string.Join(",", m_sqlSetDict.Values), m_uiDict[ID]);
                if (!runSql(sql)) {
                    break;
                }

                // remove cache
                var yoRefer = new YoCacheRefer(m_table, (table, id) => {
                    // create cache
                    var selectone = new YoSelectOne(table, m_trans);
                    selectone.GetRowTitleDisplay(id);
                });
                if (yoRefer.CheckDisplayChange(m_sqlSetDict)) {
                    yoRefer.ReferRow(m_uiDict[ID]);
                }

                result = true;
                break;
            }

            return result;
        }

        bool checkUptime() {
            bool result = false;
            while (true) {
                if (!m_yoColumnDict.ContainsKey(UPTIME)) {
                    result = true;
                    break;
                }

                // ui uptime
                if (!m_uiDict.ContainsKey(UPTIME)) {
                    break;
                }

                var uiUptime = m_uiDict[UPTIME];
                if (!YoConvert.ToDatetime(ref uiUptime)) {
                    break;
                }

                // db uptime
                var uptime = m_dataRow[UPTIME];
                if (!YoConvert.ToDatetime(ref uptime)) {
                    break;
                }

                if ((DateTime)uptime != (DateTime)uiUptime) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        bool parseSqlSet() {
            bool result = false;
            while (true) {
                if (m_uiDict == null) {
                    break;
                }

                m_errorDict = new Dictionary<string, object>();
                m_sqlSetDict = new Dictionary<string, string>();

                YoSqlHelper.EachColumn(m_yoColumnDict, (key, column) => {
                    while (true) {
                        object value = null;
                        if (!checkColumn(ref value, m_uiDict, key, column)) {
                            break;
                        }

                        if (value == null) {
                            break;
                        }

                        var dbValue = m_dataRow[key];
                        if (dbValue != null) {
                            if(value.ToString() == dbValue.ToString()) {
                                break;
                            }
                        }

                        var temp = string.Format(" `{0}`='{1}' ", key, value);
                        m_sqlSetDict.Add(key, temp);
                        break;
                    }
                });

                if(m_errorDict.Count > 0) {
                    break;
                }

                if (m_sqlSetDict.Count == 0) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

    }
}
