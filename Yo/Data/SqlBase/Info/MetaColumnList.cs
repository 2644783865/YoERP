using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class MetaColumnList : YoEngine
    {
        public MetaColumnList(string table) : base(table) { }

        public List<string> GetColumns() {
            var result = new List<string>();
            while (true) {
                var sql = string.Format("SELECT {0} FROM information_schema.columns WHERE table_schema='{1}' AND table_name='{2}';",
                    column_name, m_schema_table, m_table);
                if (!fillData(sql)) {
                    break;
                }

                foreach (DataRow row in m_dataTable.Rows) {
                    var column = row[column_name].ToString();
                    result.Add(column);
                }

                break;
            }
            return result;
        }

    }
}
