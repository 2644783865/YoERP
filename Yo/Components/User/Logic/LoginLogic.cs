using System;

namespace Yo
{
    public class LoginLogic : YoBase
    {
        UserModel m_userModel = new UserModel();
        TokenModel m_tokenModel = new TokenModel();
        public sys_token Token { get { return m_tokenModel.Row; } }

        public bool Login(string uname, string password) {
            bool result = false;
            while (true) {
                // get user
                object obj = uname;
                if (YoConvert.ToInt(ref obj)) {
                    m_userModel.FindMobile(uname);
                }
                else {
                    m_userModel.FindUname(uname);
                }

                var user = m_userModel.Row;
                if (user == null) {
                    m_errorDict[SYS] = "no user";
                    break;
                }

                password = YoConvert.ToMD5(password);
                if (user.password != password) {
                    m_errorDict[SYS] = "wrong password";
                    break;
                }

                // get token
                if (m_tokenModel.FindUid(user.id)) {
                    result = m_tokenModel.Update(m_tokenModel.Row);
                    break;
                }

                var token = new sys_token();
                token.ticket = YoConvert.ToMD5(user.id + "_" + DateTime.Now);
                token.uid = user.id;
                result = m_tokenModel.Add(token);

                break;
            }
            return result;
        }

        public bool TokenLogin(string ticket, int expiredHour) {
            bool result = false;
            while (true) {
                if (!m_tokenModel.Find(ticket)) {
                    break;
                }

                var span = DateTime.Now - m_tokenModel.Row.mtime;
                if (span.Hours > expiredHour) {
                    m_tokenModel.RemoveRow(m_tokenModel.Row);
                    break;
                }

                result = m_tokenModel.Update(m_tokenModel.Row);
                break;
            }
            return result;
        }

    }
}
