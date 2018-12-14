namespace Yo
{
    public class MetaColumn : YoEngine
    {
        const string data_type = nameof(data_type);
        const string column_type = nameof(column_type);
        const string column_comment = nameof(column_comment);
        const string format = nameof(format);

        string m_column;
        sys_column m_sysColumn;
        public sys_column SysColumn { get { return m_sysColumn; } }

        public MetaColumn(string table, string column) : base(table) {
            m_column = column;
        }

        public bool GetColumn() {
            var result = false;
            while (true) {
                m_sysColumn = null;
                var sql = string.Format("SELECT {0},{1},{2},{3} FROM information_schema.columns WHERE table_schema='{4}' AND table_name='{5}' ",
                    column_name, data_type, column_type, column_comment, m_schema_table, m_table);
                sql += string.Format(" and `{0}`='{1}'", column_name, m_column);
                if (!FindRow(sql)) {
                    break;
                }

                m_sysColumn = new sys_column();
                m_sysColumn.column_name = m_column;

                // comment
                var comment = YoConvert.Text2Json(m_dataRow[column_comment] as string);

                // format
                var formatStr = ConfigHelper.GetValue(comment, format);
                if (formatStr != null) {
                    m_sysColumn.format = formatStr.ToString();
                }

                // datatype
                m_sysColumn.datatype = YoSqlHelper.GetDataType(m_column, m_dataRow[data_type] as string, comment);
                switch (m_sysColumn.datatype) {
                    case DataType.Refer: {
                            m_sysColumn.refer = YoSqlHelper.GetReferTable(m_column);
                        }
                        break;
                    case DataType.Set: {
                            var itemArr = YoSqlHelper.GetSetList(m_dataRow[column_type]);
                            m_sysColumn.set = string.Join(",", itemArr);
                        }
                        break;
                }

                result = true;
                break;
            }
            return result;
        }

    }
}
