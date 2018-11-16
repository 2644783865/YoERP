using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoUpdate : YoSQL
    {

        DataRow m_dbRow;
        public Dictionary<string, object> m_uiDict;
        Dictionary<string, string> m_sqlSetDict;

        public YoUpdate(string table) : base(table) { }

        public bool checkUptime() {
            bool result = false;
            while (true) {
                if (!ColumnDict.ContainsKey(UPTIME)) {
                    result = true;
                    break;
                }

                // ui uptime
                if (!m_uiDict.ContainsKey(UPTIME)) {
                    break;
                }

                var uiUptime = m_uiDict[UPTIME];
                if (!YoTool.ParseDatetime(ref uiUptime)) {
                    break;
                }

                // db uptime
                var uptime = m_dbRow[UPTIME];
                if (!YoTool.ParseDatetime(ref uptime)) {
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

        public bool LoadRow(object id) {
            m_dbRow = (new YoSelectOne(m_table)).Find(id).GetRow();
            return m_dbRow != null;
        }

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

                if (!LoadRow(m_uiDict[ID])) {
                    break;
                }

                if (!checkUptime()) {
                    Message = "dirty data";
                    break;
                }

                if (!parseSqlSet()) {
                    break;
                }

                var sql = string.Format("UPDATE `{0}` SET {1} WHERE `id`='{2}';", m_table, string.Join(",", m_sqlSetDict.Values), m_uiDict[ID]);

                try {
                    using (var conn = new MySqlConnection(m_connectionString)) {
                        conn.Open();
                        var cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }

                    // remove cache
                    if (checkDirty()) {
                        m_cache.CacheRow(m_uiDict[ID]);
                    }

                    result = true;
                }
                catch (MySqlException e) {
                    Message = e.Message;
                    break;
                }

                break;
            }

            return result;
        }

        public bool checkDirty() {
            var isDirty = false;
            foreach (var field in m_titleFields) {
                if (m_sqlSetDict.ContainsKey(field)) {
                    isDirty = true;
                    break;
                }
            }
            return isDirty;
        }

        public bool parseSqlSet() {
            bool result = false;
            while (true) {
                if (m_uiDict == null) {
                    break;
                }

                ErrorList = new Dictionary<string, object>();
                m_sqlSetDict = new Dictionary<string, string>();

                YoSqlHelper.EachColumn(ColumnDict, (key, column) => {
                    while (true) {
                        object value = null;
                        if (!checkColumn(ref value, m_uiDict, key, column)) {
                            break;
                        }

                        if (value == null) {
                            break;
                        }

                        var dbValue = m_dbRow[key];
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

                if(ErrorList.Count > 0) {
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
