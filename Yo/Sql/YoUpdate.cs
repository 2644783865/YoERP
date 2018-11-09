using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Yo
{
    public class YoUpdate : YoSQL
    {
        List<string> m_listSqlSet;

        public YoUpdate(string table) : base(table) { }

        public bool Update(Dictionary<string, string> dict) {
            var result = false;
            while (true) {
                if (dict == null) {
                    break;
                }

                if (!dict.ContainsKey(ID)) {
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


        public bool parseSqlSet(Dictionary<string, string> dict) {
            if(dict == null) {
                return false;
            }

            m_listSqlSet = new List<string>();
            YoSqlHelper.EachColumn(ColumnDict, (key, val) => {
                while (true) {
                    if (key == ID) {
                        break;
                    }

                    if (!dict.ContainsKey(key)) {
                        break;
                    }

                    var temp = string.Format(" `{0}`='{1}' ", key, dict[key]);
                    m_listSqlSet.Add(temp);
                    break;
                }
            });
            return m_listSqlSet.Count > 0;
        }

    }
}
