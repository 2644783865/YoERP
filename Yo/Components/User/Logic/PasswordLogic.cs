using System;

namespace Yo
{
    public class PasswordLogic : YoBase
    {
        UserModel m_userModel = new UserModel();

        public bool Change(int uid, string passwordNew, Func<string, bool> func = null) {
            bool result = false;
            while (true) {
                if (!m_userModel.Find(uid)) {
                    break;
                }

                if (func != null && !func(m_userModel.Row.password)) {
                    break;
                }

                result = m_userModel.ChangePassword(m_userModel.Row, passwordNew);
                break;
            }
            return result;
        }

        public bool VerifyChange(int uid, string password, string passwordNew) {
            return Change(uid, passwordNew, passwordDb => {
                password = YoConvert.ToMD5(password);
                return password == passwordDb;
            });
        }

    }
}
