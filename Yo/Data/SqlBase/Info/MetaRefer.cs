using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class MetaRefer : YoEngine
    {
        public MetaRefer(string table) : base(table) { }

        public List<string> GetReferTables() {
            var result = new List<string>();
            while (true) {
                var sql = string.Format("SELECT {0} FROM information_schema.referential_constraints WHERE constraint_schema='{1}' and referenced_table_name='{2}';",
                                    table_name, m_schema_table, m_table);
                if (!fillData(sql)) {
                    break;
                }

                foreach (DataRow row in m_dataTable.Rows) {
                    result.Add(row[table_name] as string);
                }

                break;
            }
            return result;
        }

    }
}
