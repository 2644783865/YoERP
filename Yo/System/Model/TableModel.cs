using System.Collections.Generic;
using System.Linq;

namespace Yo
{
    public class TableModel : TModel<sys_table>
    {
        public bool Find(string tname) {
            if (string.IsNullOrEmpty(tname)) {
                return false;
            }

            m_row = m_db.sys_table.FirstOrDefault(r => r.table_name == tname);
            return m_row != null;
        }

        public List<sys_table> GetList() {
            return m_db.sys_table.ToList();
        }
    }
}
