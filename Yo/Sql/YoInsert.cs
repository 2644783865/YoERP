using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Yo
{
    public class YoInsert : YoSQL
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

        public bool parseSqlValues(Dictionary<string, object> dict) {
            bool result = false;
            while (true) {
                if (dict == null) {
                    break;
                }

                ErrorList = new Dictionary<string, object>();
                m_listSqlColumn = new List<string>();
                m_listSqlValue = new List<string>();

                YoSqlHelper.EachColumn(ColumnDict, (key, column) => {
                    while (true) {
                        object value = null;
                        if (!checkColumn(ref value, dict, key, column)) {
                            break;
                        }

                        m_listSqlColumn.Add(string.Format("`{0}`", key));
                        m_listSqlValue.Add(string.Format("'{0}'", value));
                        break;
                    }
                });

                if (ErrorList.Count > 0) {
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
