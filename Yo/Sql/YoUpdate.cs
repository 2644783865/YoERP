﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Yo
{
    public class YoUpdate : YoSQL
    {
        List<string> m_listSqlSet;

        public YoUpdate(string table) : base(table) { }
        
        public object getUptime(object id) {
            object result = null;
            var sql = string.Format("SELECT {0} FROM `{1}` WHERE id='{2}';", UPTIME, m_table, id);
            try {
                using (var conn = new MySqlConnection(m_connectionString)) {
                    conn.Open();
                    var cmd = new MySqlCommand(sql, conn);
                    result = cmd.ExecuteScalar();
                }
            }
            catch {
            }
            return result;
        }

        public bool checkUptime(Dictionary<string, object> dict) {
            bool result = false;
            while (true) {
                if (!ColumnDict.ContainsKey(UPTIME)) {
                    result = true;
                    break;
                }

                // ui uptime
                if (!dict.ContainsKey(UPTIME)) {
                    break;
                }

                var uiUptime = dict[UPTIME];
                if (!YoTool.ParseDatetime(ref uiUptime)) {
                    break;
                }

                // db uptime
                var uptime = getUptime(dict[ID]);
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

        public bool Update(Dictionary<string, object> dict) {
            var result = false;
            while (true) {
                if (dict == null) {
                    break;
                }

                if (!dict.ContainsKey(ID)) {
                    break;
                }

                if (!checkUptime(dict)) {
                    Message = "dirty data";
                    break;
                }

                if (!parseSqlSet(dict)) {
                    break;
                }

                var sql = string.Format("UPDATE `{0}` SET {1} WHERE `id`='{2}';", m_table, string.Join(",", m_listSqlSet), dict[ID]);

                try {
                    using (var conn = new MySqlConnection(m_connectionString)) {
                        conn.Open();
                        var cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
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

        public bool parseSqlSet(Dictionary<string, object> dict) {
            bool result = false;
            while (true) {
                if (dict == null) {
                    break;
                }

                ErrorList = new Dictionary<string, object>();
                m_listSqlSet = new List<string>();

                YoSqlHelper.EachColumn(ColumnDict, (key, column) => {
                    while (true) {
                        object value = null;
                        if (!checkColumn(ref value, dict, key, column)) {
                            break;
                        }

                        var temp = string.Format(" `{0}`='{1}' ", key, value);
                        m_listSqlSet.Add(temp);
                        break;
                    }
                });

                if(ErrorList.Count > 0) {
                    break;
                }

                if (m_listSqlSet.Count == 0) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

    }
}
