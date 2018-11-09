using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace Yo
{
    public class YoConnect
    {
        protected string m_connectionString;
        protected string m_schema_table;

        public string Message { get; set; }

        public YoConnect() {
            string schema_table = nameof(schema_table);
            string yodata = nameof(yodata);
            m_schema_table = ConfigurationManager.AppSettings[schema_table];
            m_connectionString = ConfigurationManager.AppSettings[yodata];

            if (string.IsNullOrEmpty(m_schema_table)) {
                throw new YoException(schema_table); ;
            }

            if (string.IsNullOrEmpty(m_connectionString)) {
                throw new YoException(yodata); ;
            }

            m_connectionString += m_schema_table;
        }

        public bool fill(string sql, Action<DataSet> act) {
            var result = false;
            while (true) {
                if (string.IsNullOrEmpty(sql)) {
                    break; ;
                }

                if (act == null) {
                    break; ;
                }

                try {
                    using (var conn = new MySqlConnection(m_connectionString)) {
                        var cmd = new MySqlCommand(sql, conn);
                        var da = new MySqlDataAdapter(cmd);
                        var ds = new DataSet();
                        da.Fill(ds);
                        act(ds);
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

    }
}
