using System;
using System.Linq;

namespace Yo
{
    public class TokenModel : TModel<sys_token>
    {
        public bool Add(sys_token row) {
            if (!check(row)) {
                return false;
            }
            row.ctime = DateTime.Now;
            row.mtime = DateTime.Now;
            return AddRow(row);
        }

        public bool Update(sys_token row) {
            if (!check(row)) {
                return false;
            }
            row.mtime = DateTime.Now;
            return ModifyRow(row);
        }

        public bool Find(string ticket) {
            if (string.IsNullOrEmpty(ticket)) {
                return false;
            }

            m_row = m_db.sys_token.FirstOrDefault(r => r.ticket == ticket);
            return m_row != null;
        }

        public bool FindUid(int uid) {
            m_row = m_db.sys_token.FirstOrDefault(r => r.uid == uid);
            return m_row != null;
        }

    }
}
