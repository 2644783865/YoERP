using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoCacheRefer : YoSQL
    {
        Action<string, object> m_act;
        public YoCacheRefer(string table, Action<string, object> act) : base(table) {
            m_act = act;
        }

        public bool CheckDisplayChange(Dictionary<string, string> sqlSetDict) {
            var isChanged = false;
            foreach (var field in m_titleFields) {
                if (sqlSetDict.ContainsKey(field)) {
                    isChanged = true;
                    break;
                }
            }
            return isChanged;
        }

        void referTables(object id) {
            if(m_sysTable == null || m_sysTable.refer == null) {
                return;
            }

            var key = m_table + "_" + ID;
            var sqlSetDict = new Dictionary<string, string>();
            sqlSetDict.Add(key, null);

            var tableList = YoConvert.ToList(m_sysTable.refer);
            foreach (var table in tableList) {
                var refer = new YoCacheRefer(table, m_act);

                if (!refer.CheckDisplayChange(sqlSetDict)) {
                    continue;
                }
                refer.referRows(key, id);
            }
        }

        void referRows(object key, object value) {
            var sql = string.Format("SELECT {0} FROM `{1}` WHERE `{2}`='{3}'", ID, m_table, key, value);
            if (!fillData(sql)) {
                return;
            }

            foreach (DataRow row in m_dataTable.Rows) {
                ReferRow(row[ID]);
            }
        }

        public void ReferRow(object id) {
            if (m_cache.Set(id, null)) {
                referTables(id);
                if (m_act != null) {
                    m_act(m_table, id);
                }
            }
        }
    }
}
