using System;
using System.Data.Entity;

namespace Yo
{
    public class TModel<T> : YoBase where T : class
    {
        protected T m_row;
        public T Row { get { return m_row; } }
        public bool HasRow { get { return m_row != null; } }

        protected JDb m_db;
        protected DbSet<T> m_dbset;

        public TModel() {
            m_db = new JDb();
            var property = typeof(JDb).GetProperty(typeof(T).Name);
            m_dbset = (DbSet<T>)property.GetValue(m_db);
        }

        protected bool saveChanges() {
            bool result = false;
            while (true) {
                m_errorDict.Clear();
                try {
                    m_db.SaveChanges();
                    result = true;
                }
                catch (Exception e) {
                    m_errorDict[DB] = e.Message;
                }
                break;
            }

            return result;
        }

        protected bool check(T row) {
            if (row == null) {
                return false;
            }
            return true;
        }

        public bool AddRow(T row) {
            if (row == null) {
                return false;
            }
            m_row = m_dbset.Add(row);
            return saveChanges();
        }

        public bool ModifyRow(T row) {
            if (row == null) {
                return false;
            }
            m_row = m_dbset.Attach(row);
            m_db.Entry(m_row).State = EntityState.Modified;
            return saveChanges();
        }

        public bool RemoveRow(T row) {
            if (row == null) {
                return false;
            }
            m_db.Entry(row).State = EntityState.Deleted;
            return saveChanges();
        }

        public T FindRow(int id) {
            return m_dbset.Find(id);
        }

    }
}
