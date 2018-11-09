using System.Data;

namespace Yo
{
    public class YoSelect : YoSQL
    {
        public YoSelect(string table) : base(table) { }

        public DataView GetView() {
            DataView view = null;
            var sql = string.Format("SELECT * FROM `{0}`;", m_table);

            fill(sql, ds => {
                view = YoSqlHelper.GetView(ds);
            });
            return view;
        }

        public DataRowCollection GetRows() {
            DataRowCollection rows = null;
            var sql = string.Format("SELECT * FROM `{0}`;", m_table);

            fill(sql, ds => {
                rows = YoSqlHelper.GetRows(ds);
            });
            return rows;
        }

    }
}
