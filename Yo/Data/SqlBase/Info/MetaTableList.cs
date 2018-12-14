using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class MetaTableList : YoEngine
    {
        const string sys_ = nameof(sys_);

        public List<string> GetTables() {
            var result = new List<string>();
            while (true) {
                var sql = string.Format("SELECT {0} FROM information_schema.tables WHERE table_schema='{1}'",
                            table_name, m_schema_table);
                if (!fillData(sql)) {
                    break;
                }

                foreach (DataRow row in m_dataTable.Rows) {
                    var table = row[table_name].ToString();
                    if (table.StartsWith(sys_)) {
                        continue;
                    }

                    result.Add(table);
                }

                break;
            }
            return result;
        }
        
    }
}
