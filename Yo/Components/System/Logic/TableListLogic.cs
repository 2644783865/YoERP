namespace Yo
{
    public class TableListLogic : YoBase
    {
        public void SyncTableList() {
            var metaTables = (new MetaTableList()).GetTables();

            // SyncStructure
            foreach (var table in metaTables) {
                (new TableLogic(table)).SyncStructure();
            }

            // SyncStructureRefer
            foreach (var table in metaTables) {
                (new TableLogic(table)).SyncStructureRefer();
            }

            // drop table
            var sysTables = (new TableModel()).GetList();
            foreach (var sysTable in sysTables) {
                if (metaTables.Contains(sysTable.table_name)) {
                    continue;
                }

                (new TableLogic(sysTable.table_name)).RemoveTable();
            }
        }

    }
}
