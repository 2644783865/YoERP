using System.Data;

namespace Yo
{
    public class YoSelect : YoSQL
    {
        public YoSelect(string table) : base(table) { }

        public DataView GetView() {
            DataView view = null;
            var sql = string.Format("SELECT * FROM `{0}`;", m_table);

            if (getData(sql)) {
                view = YoSqlHelper.GetView(m_dataTable);
            }
            return view;
        }

        public DataRowCollection GetRows() {
            DataRowCollection rows = null;
            var sql = string.Format("SELECT * FROM `{0}`;", m_table);

            if (getData(sql)) {
                rows = YoSqlHelper.GetRows(m_dataTable);
            }
            return rows;
        }

    }
}
