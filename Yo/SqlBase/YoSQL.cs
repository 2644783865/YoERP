using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Yo
{
    public class YoSQL : YoConnect
    {
        protected const string ID = "id";
        protected const string table_display = "display";

        protected string m_table;
        public Dictionary<string, yo_column> ColumnDict;

        public YoSQL(string table) {
            m_table = table;
            var obj = new YoColumn();
            ColumnDict = obj.GetColumns(m_table);
        }

    }
}
