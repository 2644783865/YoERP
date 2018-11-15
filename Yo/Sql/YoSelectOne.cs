using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSelectOne : YoSelect
    {
        public YoSelectOne(string table) : base(table) { }

        public YoSelectOne Find(object id) {
            var sql = string.Format("SELECT * FROM `{0}` WHERE id='{1}';", m_table, id);
            getData(sql);
            return this;
        }

        public bool hasRow() {
            var result = false;
            while (true) {
                if(m_dataTable == null) {
                    break;
                }

                if(m_dataTable.Rows.Count == 0) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        public DataRow GetRow() {
            if (!hasRow()) {
                return null;
            }
            return m_dataTable.Rows[0];
        }

        public DataRow GetRowDisplay() {
            if (!hasRow()) {
                return null;
            }

            var displayTable = GetDisplayTable();
            return displayTable.Rows[0];

        }

        public Dictionary<string, yo_ui_column> GetUIColumnDict(object trans = null, bool isModify = true) {
            var uiColumnDict = new Dictionary<string, yo_ui_column>();
            if (isModify && !hasRow()) {
                return uiColumnDict;
            }

            var row = GetRow();
            var rowDisplay = GetRowDisplay();

            foreach(var column in ColumnDict.Values) {
                var ui_column = new yo_ui_column();
                var key = column.column_name;
                ui_column.isPK = key == ID;
                ui_column.key = key;
                ui_column.keyDisplay = ConfigHelper.Translate(key, trans);
                ui_column._set = column._set;
                if (isModify) {
                    ui_column.value = row[key];
                    ui_column.valueDisplay = rowDisplay[key];
                }
                if (YoSqlHelper.IsRelation(key)) {
                    ui_column.refer = YoSqlHelper.GetRelationTable(key);
                }
                uiColumnDict.Add(key, ui_column);
            }
            return uiColumnDict;
        }

    }
}
