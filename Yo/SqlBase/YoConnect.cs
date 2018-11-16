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
        protected DataTable m_dataTable;

        protected const string ID = "id";
        protected const string SCHEMA_TABLE = "schema_table";
        protected const string CACHE_TABLE = "cache_table";
        protected const string YODATA = "yodata";

        public string Message { get; set; }
        public DataTable DataTable { get { return m_dataTable; } }

        public YoConnect() {
            LoadConfig(SCHEMA_TABLE);
        }

        public void LoadConfig(string schema_table, string yodata = YODATA) {
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

        public bool getData(string sql) {
            var result = false;
            while (true) {
                disposeDataTable();

                if (string.IsNullOrEmpty(sql)) {
                    break; ;
                }

                try {
                    var ds = new DataSet();
                    using (var conn = new MySqlConnection(m_connectionString)) {
                        var cmd = new MySqlCommand(sql, conn);
                        var da = new MySqlDataAdapter(cmd);
                        da.Fill(ds);
                    }

                    if (ds == null) {
                        break;
                    }

                    if (ds.Tables.Count == 0) {
                        break;
                    }

                    m_dataTable = ds.Tables[0];
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

        void disposeDataTable() {
            if (m_dataTable != null) {
                m_dataTable.Dispose();
                m_dataTable = null;
            }
        }

    }
}
