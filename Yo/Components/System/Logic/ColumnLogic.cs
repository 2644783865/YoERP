namespace Yo
{
    public class ColumnLogic : YoBase
    {
        int m_tableId;
        string m_column;
        MetaColumn m_metaColumn;
        ColumnModel m_columnModel;

        public ColumnLogic(int tableId, string column) {
            m_columnModel = new ColumnModel();
            var sysTable = (new TableModel()).FindRow(tableId);
            if (sysTable == null) {
                return;
            }
            m_tableId = tableId;
            m_column = column;
            m_metaColumn = new MetaColumn(sysTable.table_name, m_column);
            m_columnModel.Find(sysTable.id, m_column);
        }

        public bool SyncStructure(int sort = 0) {
            var result = false;
            while (true) {
                if (m_column == null) {
                    break;
                }

                if (!m_metaColumn.GetColumn()) {
                    break;
                }

                var metaSysColumn = m_metaColumn.SysColumn;
                metaSysColumn.sys_table_id = m_tableId;
                metaSysColumn.sort = sort;
                if (!m_columnModel.HasRow) {
                    result = m_columnModel.AddRow(metaSysColumn);
                    break;
                }

                metaSysColumn.id = m_columnModel.Row.id;
                result = (new ColumnModel()).ModifyRow(metaSysColumn);
                break;
            }
            return result;
        }

        public bool RemoveColumn() {
            var result = false;
            while (true) {
                if (!m_columnModel.HasRow) {
                    break;
                }

                m_columnModel.RemoveRow(m_columnModel.Row);
                result = true;
                break;
            }
            return result;
        }

    }
}
