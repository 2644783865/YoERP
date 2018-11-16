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
            string result = null;
            while (true) {
                // get cache
                var temp = m_cache.Get(id);
                if(temp != null) {
                    return temp.ToString();
                }

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

                var titleList = new List<string>();
                foreach (var field in m_titleFields) {
                    string title = null;
                    if (YoSqlHelper.IsRelation(field)) {
                        var table = YoSqlHelper.GetRelationTable(field);
                        var select = new YoSelect(table);
                        title = select.GetDisplay(row[field]);
                    }
                    else {
                        title = row[field].ToString();
                    }
                    titleList.Add(title);
                }

                result = string.Join("_", titleList);
                break;
            }

            // set cache
            if (result != null) {
                m_cache.Set(id, result);
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
                    displayTable.Columns.Add(new DataColumn(key, typeof(object)));
                });

                foreach (DataRow row in m_dataTable.Rows) {
                    var rowNew = displayTable.NewRow();
                    displayTable.Rows.Add(rowNew);

                    YoSqlHelper.EachColumn(ColumnDict, (key, column) => {
                        if (YoSqlHelper.IsRelation(key)) {
                            var table = YoSqlHelper.GetRelationTable(key);
                            var select = new YoSelect(table);
                            rowNew[key] = select.GetDisplay(row[key]);
                        }
                        else {
                            rowNew[key] = db2ui(row[key], column);
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
