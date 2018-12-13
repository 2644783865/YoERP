namespace Yo
{
    public class YoCacheManage : YoEngine
    {
        string m_table;

        public YoCacheManage(string table) {
            m_table = table;
        }

        public bool InitData() {
            bool result = true;
            while (true) {
                const string sys_cachetitle = nameof(sys_cachetitle);
                const string table_name = nameof(table_name);
                const string init = nameof(init);

                // get table_name
                var sqlGet = string.Format("SELECT * FROM `{0}` where table_name='{1}'", sys_cachetitle, m_table);
                var row = getFirst(sqlGet);
                if (row != null) {
                    break;
                }

                // init data
                var select = new YoSelect(this.m_table);
                select.Select().GetTableTitleDisplay();

                // set table_name
                string[] keys = new string[] { table_name, init };
                for(var i = 0; i< keys.Length; ++i) {
                    keys[i] = string.Format("`{0}`", keys[i]);
                }
                string[] values = new string[] { m_table, "1" };
                for (var i = 0; i < values.Length; ++i) {
                    values[i] = string.Format("'{0}'", values[i]);
                }

                var sqlInsert = string.Format("INSERT INTO `{0}` ({1}) VALUES ({2}); "
                    , sys_cachetitle, string.Join(",", keys), string.Join(",", values));
                result = runSql(sqlInsert);
                break;
            }
            return result;
        }

    }
}
