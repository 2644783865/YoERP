using System.Collections.Generic;

namespace Yo
{
    public class YoSQL : YoEngine
    {
        protected const string UPTIME = "uptime";

        protected object m_trans;
        protected sys_table m_sysTable;
        protected Dictionary<string, sys_column> m_yoColumnDict;
        protected List<string> m_titleFields;
        protected IYoCache m_cache;

        public Dictionary<string, sys_column> YoColumnDict { get { return m_yoColumnDict; } }
        public sys_table YoTable { get { return m_sysTable; } }

        public YoSQL(string table, object trans = null) {
            m_table = table;
            m_trans = trans;
            var tableModel = new TableModel();
            if (tableModel.Find(table)) {
                m_cache = new DbCache(m_table);
                m_sysTable = tableModel.Row;
                init();
            }
        }

        void init() {
            if (m_sysTable.title != null) {
                m_titleFields = YoConvert.ToList(m_sysTable.title);
                m_sysTable.translation = ConfigHelper.Translate(m_sysTable.table_name, m_trans);
            }

            var yoColumnList = (new ColumnModel()).GetList(m_sysTable.id);
            m_yoColumnDict = YoConvert.List2Dict(yoColumnList, column_name, yoColumn => {
                if (yoColumn.set != null) {
                    var setList = YoConvert.ToList(yoColumn.set);
                    yoColumn.setDict = new Dictionary<string, string>();
                    foreach (var setKey in setList) {
                        var setValue = ConfigHelper.Translate(setKey, m_trans);
                        yoColumn.setDict.Add(setKey, setValue);
                    }
                }
                yoColumn.translation = ConfigHelper.Translate(yoColumn.column_name, m_trans);
                return yoColumn;
            });
        }

        public bool Find(object id) {
            var sql = string.Format("SELECT * FROM `{0}` where id='{1}'", m_table, id);
            return FindRow(sql);
        }

    }
}
