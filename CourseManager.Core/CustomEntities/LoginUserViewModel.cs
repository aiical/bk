using System;
using System.Collections.Generic;

namespace iFuturePortal.EntitiesFromCustom
{
    public class LoginUserViewModel
    {
        public string UserId { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public string CnName { get; set; }
        public string EnName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string CheckCode { get; set; }
        public int ChiefId { get; set; }
        public List<int> Roles { get; set; }
        public string OpenId { get; set; }

    }
}
