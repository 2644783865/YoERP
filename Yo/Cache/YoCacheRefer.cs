using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoCacheRefer : YoSQL
    {
        public YoCacheRefer(string table) : base(table) { }

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

        public void ReferTables(object id) {
            var TABLENAME = "table_name";
            var sql = string.Format("SELECT {0} FROM information_schema.referential_constraints WHERE constraint_schema='{1}' and referenced_table_name='{2}';",
                                TABLENAME, m_schema_table, m_table);
            if (!fillData(sql)) {
                return;
            }

            var key = m_table + "_" + ID;
            var sqlSetDict = new Dictionary<string, string>();
            sqlSetDict.Add(key, null);
            foreach (DataRow row in m_dataTable.Rows) {
                var table = row[TABLENAME].ToString();
                var refer = new YoCacheRefer(table);

                if (!refer.CheckDisplayChange(sqlSetDict)) {
                    continue;
                }
                refer.ReferRows(key, id);
            }
        }

        public void ReferRows(object key, object value) {
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
                ReferTables(id);
            }
        }
    }
}
