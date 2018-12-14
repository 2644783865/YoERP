using System.Collections.Generic;
using System.Linq;

namespace Yo
{
    public class ColumnModel : TModel<sys_column>
    {
        public bool Find(int table_id, string column) {
            if (string.IsNullOrEmpty(column)) {
                return false;
            }

            m_row = m_db.sys_column.FirstOrDefault(r => r.sys_table_id == table_id && r.column_name == column);
            return m_row != null;
        }

        public List<sys_column> GetList(int table_id) {
            return m_db.sys_column.Where(r => r.sys_table_id == table_id).OrderBy(r => r.sort).ToList();
        }
    }
}
