using System.Linq;

namespace Yo
{
    public class UserModel : TModel<sys_user>
    {
        public bool Add(sys_user row) {
            if (!check(row)) {
                return false;
            }
            row.password = YoConvert.ToMD5(row.password);
            return AddRow(row);
        }

        public bool ChangePassword(sys_user row, string passwordNew) {
            row.password = passwordNew;
            if (!check(row)) {
                return false;
            }
            row.password = YoConvert.ToMD5(row.password);
            return ModifyRow(row);
        }

        public bool Find(int id) {
            m_row = m_db.sys_user.FirstOrDefault(r => r.id == id);
            return m_row != null;
        }

        public bool FindUname(string uname) {
            if (string.IsNullOrEmpty(uname)) {
                return false;
            }

            m_row = m_db.sys_user.FirstOrDefault(r => r.uname == uname);
            return m_row != null;
        }

        public bool FindMobile(string mobile) {
            if (string.IsNullOrEmpty(mobile)) {
                return false;
            }

            m_row = m_db.sys_user.FirstOrDefault(r => r.mobile == mobile);
            return m_row != null;
        }

    }
}
