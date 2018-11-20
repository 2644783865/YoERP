using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSelect : YoSelectOne
    {
        public YoSelect(string table, object trans = null) : base(table, trans) { }

        public YoSelect Select() {
            var sql = string.Format("SELECT * FROM `{0}`", m_table);
            fillData(sql);
            return this;
        }

        public DataTable GetTableDisplay() {
            DataTable displayTable = null;
            while (true) {
                if(m_dataTable == null) {
                    break;
                }

                displayTable = new DataTable();
                YoSqlHelper.EachColumn(m_yoColumnDict, (key, val) => {
                    displayTable.Columns.Add(new DataColumn(key, typeof(string)));
                });

                foreach (DataRow row in m_dataTable.Rows) {
                    var rowNew = displayTable.NewRow();
                    displayTable.Rows.Add(rowNew);

                    YoSqlHelper.EachColumn(m_yoColumnDict, (key, yoColumn) => {
                        rowNew[key] = GetColumnDisplay(row[key], yoColumn);
                    });
                }

                break;
            }

            return displayTable;
        }

    }
}
