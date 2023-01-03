using BOTY.Models.Tables;
using System.Collections.Generic;

namespace BOTY.Models.zabezpečení
{
    public class UsersService
    {
        public List<logins> admins { get; set; }
        public bool Authenticate(string login, string password)
        {
            foreach (var item in admins)
            {
                if (login == item.login && password == item.password)
                    return true;
            }

            return false;
        }
    }
}
