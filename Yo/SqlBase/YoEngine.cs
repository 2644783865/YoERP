using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Yo
{
    public class YoEngine
    {
        protected const string ID = "id";
        protected const string DB = "db";
        protected const string CACHE = "_cache";
        protected const string TITLE = "title";

        protected const string SCHEMA_TABLE = "schema_table";
        protected const string CACHE_TABLE = "cache_table";
        protected const string YODATA = "yodata";

        protected string m_schema_table;
        protected string m_connectionString;
        protected DataTable m_dataTable;
        protected DataRow m_dataRow;
        protected Dictionary<string, object> m_errorDict;

        public DataRowCollection Rows { get { return (m_dataTable != null) ? m_dataTable.Rows : null; } }
        public DataView View { get { return (m_dataTable != null) ? m_dataTable.DefaultView : null; } }
        public DataRow Row { get { return m_dataRow; } }
        public Dictionary<string, object> ErrorDict { get { return m_errorDict; } }

        public YoEngine() {
            LoadConfig(SCHEMA_TABLE);
            m_errorDict = new Dictionary<string, object>();
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

        public bool FindRow(string sql) {
            var obj = new YoEngine();
            if (!obj.fillData(sql + " limit 1")) {
                return false;
            }
            m_dataRow = obj.Rows[0];
            return true;
        }

        protected bool fillData(string sql) {
            var result = false;
            while (true) {
                m_errorDict.Clear();
                if (m_dataTable != null) {
                    m_dataTable.Dispose();
                    m_dataTable = null;
                }

                if (string.IsNullOrEmpty(sql)) {
                    break;
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

                    var dataTable = ds.Tables[0];
                    if (dataTable.Rows.Count == 0) {
                        break;
                    }

                    m_dataTable = dataTable;
                    result = true;
                }
                catch (MySqlException e) {
                    m_errorDict[DB] = e.Message;
                    break;
                }

                break;
            }

            return result;
        }

        protected bool runSql(string sql) {
            var result = false;
            while (true) {
                m_errorDict.Clear();
                if (string.IsNullOrEmpty(sql)) {
                    break;
                }

                try {
                    using (var conn = new MySqlConnection(m_connectionString)) {
                        conn.Open();
                        var cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    result = true;
                }
                catch (MySqlException e) {
                    m_errorDict[DB] = e.Message;
                    break;
                }

                break;
            }

            return result;
        }

        protected object getFirst(string sql) {
            object result = null;
            while (true) {

                try {
                    using (var conn = new MySqlConnection(m_connectionString)) {
                        conn.Open();
                        var cmd = new MySqlCommand(sql, conn);
                        result = cmd.ExecuteScalar();
                    }
                }
                catch (MySqlException e) {
                    m_errorDict[DB] = e.Message;
                    break;
                }

                break;
            }
            return result;
        }

    }
}
