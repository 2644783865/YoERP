using Newtonsoft.Json.Linq;
using System.Data;

namespace Yo
{
    public class YoSelect : YoSelectOne
    {
        const string k1 = nameof(k1);
        const string k2 = nameof(k2);

        string m_orderby;
        string m_limit;
        string m_where;

        public YoSelect(string table, object trans = null) : base(table, trans) { }

        public YoSelect Select() {
            var sql = string.Format("SELECT * FROM `{0}` {1} {2} {3}", m_table, m_where, m_orderby, m_limit);
            fillData(sql);
            return this;
        }

        public int GetCount() {
            var result = 0;
            var sql = string.Format("SELECT COUNT(1) FROM `{0}` {1}", m_table, m_where);
            var obj = getFirst(sql);

            if (YoConvert.ToInt(ref obj)) {
                result = (int)obj;
            }
            return result;
        }

        string getWhere(string key, JObject obj) {
            string result = null;
            while (true) {
                if (string.IsNullOrEmpty(key)) {
                    break;
                }

                if (!m_yoColumnDict.ContainsKey(key)) {
                    break;
                }

                if (obj == null) {
                    break;
                }

                object v1 = obj[k1];
                object v2 = obj[k2];
                switch (m_yoColumnDict[key].datatype) {
                    case DataType.ID:
                    case DataType.Refer:
                    case DataType.Number:
                    case DataType.Calc: {
                            if(v1 != null && YoConvert.ToDouble(ref v1)) {
                                result += string.Format(" `{0}` >= {1} ", key, v1);
                            }

                            if (v2 != null && YoConvert.ToDouble(ref v2)) {
                                if(result != null) {
                                    result += "AND";
                                }
                                result += string.Format(" `{0}` <= {1} ", key, v2);
                            }

                        }
                        break;
                    default: {
                            if (v1 != null && YoConvert.ToString(ref v1)) {
                                result += string.Format(" `{0}` like '%{1}%' ", key, v1);
                            }
                        }
                        break;
                }

                break;
            }
            return result;
        }

        public YoSelect Where(JObject obj) {
            while (true) {
                if (obj == null) {
                    break;
                }

                string where = null;
                string pre = null;
                ConfigHelper.JObjectForeach(obj, (key, val) => {
                    while (true) {
                        if (key == null) {
                            break;
                        }

                        var temp = getWhere(key.ToString(), (JObject)val);
                        if (temp == null) {
                            break;
                        }

                        pre = pre == null ? " WHERE " : " AND ";
                        where += pre + temp;
                        break;
                    }
                });

                m_where = where;
                break;
            }
            return this;
        }

        public YoSelect Limit(JObject obj) {
            while (true) {
                if (obj == null) {
                    break;
                }

                object v1 = obj[k1];
                if (v1 == null || !YoConvert.ToInt(ref v1)) {
                    break;
                }

                object v2 = obj[k2];
                if (v2 != null) {
                    if(!YoConvert.ToInt(ref v2)) {
                        v2 = null;
                    }
                }

                var temp = v2 != null ? ", " : null;

                m_limit = string.Format(" LIMIT {0}{1}{2} ", v1, temp, v2);
                break;
            }
            return this;
        }

        public YoSelect OrderBy(JObject obj) {
            while (true) {
                if(obj == null) {
                    break;
                }

                object v1 = obj[k1];
                if (v1 == null) {
                    break;
                }

                if (!m_yoColumnDict.ContainsKey(v1.ToString())) {
                    break;
                }

                var sequence = obj[k2] == null ? "ASC" : "DESC";
                m_orderby = string.Format(" ORDER BY `{0}` {1} ", v1, sequence);

                break;
            }
            return this;
        }

        public DataTable GetTableDisplay() {
            DataTable displayTable = null;
            while (true) {
                if(m_dataTable == null) {
                    break;
                }

                displayTable = new DataTable();
                YoSqlHelper.EachColumn(m_yoColumnDict, (key, val) => {
                    displayTable.Columns.Add(new DataColumn(key, typeof(string)));
                });

                foreach (DataRow row in m_dataTable.Rows) {
                    var rowNew = displayTable.NewRow();
                    displayTable.Rows.Add(rowNew);

                    YoSqlHelper.EachColumn(m_yoColumnDict, (key, yoColumn) => {
                        rowNew[key] = getColumnDisplay(row[key], yoColumn);
                    });
                }

                break;
            }

            return displayTable;
        }

        public DataTable GetTableTitleDisplay() {
            DataTable titleDisplayTable = null;
            while (true) {
                if (m_dataTable == null) {
                    break;
                }

                titleDisplayTable = new DataTable();
                titleDisplayTable.Columns.Add(new DataColumn(ID, typeof(int)));
                titleDisplayTable.Columns.Add(new DataColumn(TITLE, typeof(string)));

                foreach (DataRow row in m_dataTable.Rows) {
                    var rowNew = titleDisplayTable.NewRow();
                    titleDisplayTable.Rows.Add(rowNew);
                    rowNew[ID] = row[ID];
                    rowNew[TITLE] = GetRowTitleDisplay(row, true);
                }

                break;
            }

            return titleDisplayTable;
        }

    }
}
