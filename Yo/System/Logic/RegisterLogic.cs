using System;
using System.Collections.Generic;

namespace Yo
{
    public class RegisterLogic : YoBase
    {
        UserModel m_userModel = new UserModel();

        public bool Register(Dictionary<string, object> dict, string code) {
            if (DateTime.Now.Day.ToString() != code) {
                return false;
            }

            var user = YoConvert.Dict2Class<sys_user>(dict);
            return m_userModel.Add(user);
        }

    }
}
