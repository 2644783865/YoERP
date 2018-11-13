using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSelect : YoSQL
    {
        public YoSelect(string table) : base(table) { }

        public YoSelect Select() {
            var sql = string.Format("SELECT * FROM `{0}`;", m_table);
            getData(sql);
            return this;
        }

        public string GetDisplay(object id, bool isRow = false) {
            string result = "";
            while (true) {
                DataRow row = null;
                if (isRow) {
                    row = id as DataRow;
                }
                else {
                    var sql = string.Format("SELECT * FROM `{0}` WHERE id={1} limit 1;", m_table, id);

                    if (!getData(sql)) {
                        break;
                    }

                    var rows = YoSqlHelper.GetRows(m_dataTable);
                    if (rows == null) {
                        break;
                    }

                    row = rows[0];
                }

                if(row == null) {
                    break;
                }

                var format = (new YoTable()).GetComment(m_table) as string;

                if (!string.IsNullOrEmpty(format)) {
                    var tableList = format.Split(',');
                    var displayList = new List<string>();
                    foreach (var table in tableList) {
                        var select = new YoSelect(table);
                        displayList.Add(select.GetDisplay(row[table + "_id"]));
                    }
                    result = string.Join("_", displayList);
                    break;
                }

                foreach (var key in ColumnDict.Keys) {
                    if (key == ID) {
                        continue;
                    }

                    if (YoSqlHelper.IsRelation(key)) {
                        var table = YoSqlHelper.GetRelationTable(key);
                        var select = new YoSelect(table);
                        result = select.GetDisplay(row[key]);
                    }
                    else {
                        result = row[key].ToString();
                    }
                    break;
                }

                break;
            }

            return result;
        }

        public DataTable GetDisplayTable() {
            DataTable displayTable = null;
            while (true) {
                if(m_dataTable == null) {
                    break;
                }

                displayTable = new DataTable();
                YoSqlHelper.EachColumn(ColumnDict, (key, val) => {
                    displayTable.Columns.Add(new DataColumn(key, typeof(string)));
                });

                foreach (DataRow row in m_dataTable.Rows) {
                    var rowNew = displayTable.NewRow();
                    displayTable.Rows.Add(rowNew);

                    YoSqlHelper.EachColumn(ColumnDict, (key, val) => {
                        if (YoSqlHelper.IsRelation(key)) {
                            var table = YoSqlHelper.GetRelationTable(key);
                            var select = new YoSelect(table);
                            rowNew[key] = select.GetDisplay(row[key]);
                        }
                        else {
                            rowNew[key] = row[key].ToString();
                        }
                    });
                }

                break;
            }

            return displayTable;
        }

        public List<yo_ui_row> GetUIRows() {
            var uiRowList = new List<yo_ui_row>();
            while (true) {
                if (m_dataTable == null) {
                    break;
                }
                
                foreach (DataRow row in m_dataTable.Rows) {
                    var uiRow = new yo_ui_row();
                    uiRowList.Add(uiRow);

                    uiRow.value = row[ID];
                    uiRow.valueDisplay = GetDisplay(row, true);
                }

                break;
            }

            return uiRowList;
        }

    }
}
