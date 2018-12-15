namespace Yo
{
    public class TableLogic : YoBase
    {
        const string _id = nameof(_id);

        string m_table;
        MetaTable m_metaTable;
        MetaTitleTable m_metaTitleTable;
        TableModel m_tableModel;

        public TableLogic(string table) {
            m_table = table;
            m_metaTable = new MetaTable(m_table);
            m_metaTitleTable = new MetaTitleTable(m_table);
            m_tableModel = new TableModel();
            m_tableModel.Find(m_table);
        }

        // 1
        public bool SyncStructure() {
            var result = false;
            while (true) {
                if (!m_metaTable.GetTable()) {
                    break;
                }

                if(m_metaTable.SysTable.title == null) {
                    var columns = (new MetaColumnList(m_table)).GetColumns();
                    if(columns.Count > 1) {
                        m_metaTable.SysTable.title = columns[1];
                    }
                }

                if (!m_metaTitleTable.HasTable()) {
                    m_metaTitleTable.CreateTable();
                }

                if (!m_tableModel.HasRow) {
                    result = m_tableModel.AddRow(m_metaTable.SysTable);
                    break;
                }

                var sysTable = m_tableModel.Row;
                var metaSysTable = m_metaTable.SysTable;
                if (sysTable.title != metaSysTable.title) {
                    m_metaTitleTable.CleanTable();
                }
                else {
                    metaSysTable.init_title = sysTable.init_title;
                }

                metaSysTable.id = sysTable.id;
                result = (new TableModel()).ModifyRow(metaSysTable);
                break;
            }

            if (result) {
                (new ColumnListLogic(m_tableModel.Row.id)).SyncColumnList();
            }
            return result;
        }

        // 2
        public bool SyncStructureRefer() {
            var result = false;
            while (true) {
                if (!m_tableModel.HasRow) {
                    break;
                }

                if (m_tableModel.Row.init_title) {
                    result = true;
                    break;
                }

                var referList = YoConvert.ToList(m_tableModel.Row.refer);
                foreach(var table in referList) {
                    var tableLogic = new TableLogic(table); ;
                    tableLogic.SyncStructureReferField(m_table + _id);
                }

                result = true;
                break;
            }
            return result;
        }


        // 2.1
        public bool SyncStructureReferField(string field) {
            var result = false;
            while (true) {
                if (string.IsNullOrEmpty(field)) {
                    break;
                }

                if (!m_tableModel.HasRow) {
                    break;
                }

                var fieldList = YoConvert.ToList(m_tableModel.Row.title);
                if (!fieldList.Contains(field)) {
                    break;
                }

                m_metaTitleTable.CleanTable();
                m_tableModel.Row.init_title = false;
                result = m_tableModel.ModifyRow(m_tableModel.Row);
                break;
            }
            return result;

        }

        // 3
        public bool SyncTitle() {
            var result = false;
            while (true) {
                if (!m_tableModel.HasRow) {
                    break;
                }

                if (m_tableModel.Row.init_title) {
                    result = true;
                    break;
                }

                // create data
                var select = new YoSelect(m_table);
                select.Select().GetTableTitleDisplay();

                m_tableModel.Row.init_title = true;
                result = m_tableModel.ModifyRow(m_tableModel.Row);
                break;
            }
            return result;
        }

        public bool RemoveTable() {
            var result = false;
            while (true) {
                if (!m_tableModel.HasRow) {
                    break;
                }

                var tableId = m_tableModel.Row.id;
                var sysColumnList = (new ColumnModel()).GetList(tableId);
                foreach(var sysColumn in sysColumnList) {
                    (new ColumnLogic(tableId, sysColumn.column_name)).RemoveColumn();
                }
                m_tableModel.RemoveRow(m_tableModel.Row);
                m_metaTitleTable.DropTable();

                result = true;
                break;
            }
            return result;
        }

    }
}
