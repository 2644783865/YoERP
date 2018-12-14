namespace Yo
{
    public class ColumnListLogic : YoBase
    {
        sys_table m_sysTable;

        public ColumnListLogic(int tableId) {
            m_sysTable = (new TableModel()).FindRow(tableId);
        }

        public void SyncColumnList() {
            if (m_sysTable == null) {
                return;
            }

            var metaColumns = (new MetaColumnList(m_sysTable.table_name)).GetColumns();

            // SyncStructure
            var i = 0;
            foreach (var column in metaColumns) {
                (new ColumnLogic(m_sysTable.id, column)).SyncStructure(++i);
            }

            // remove column
            var sysColumns = (new ColumnModel()).GetList(m_sysTable.id);
            foreach (var sysColumn in sysColumns) {
                if (metaColumns.Contains(sysColumn.column_name)) {
                    continue;
                }

                (new ColumnLogic(m_sysTable.id, sysColumn.column_name)).RemoveColumn();
            }
        }

    }
}
