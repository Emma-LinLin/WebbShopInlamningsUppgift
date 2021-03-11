using System;
using System.Collections.Generic;
using System.Text;
using WebbShopInlamningsUppgift.API;

namespace WebbShopInlamningsUppgift.Controllers
{
    class Test
    {
        public static void Run()
        {
            var api = new WebbShopAPI();

            int userId = api.Login("TestCustomer", "Codic2021");
            Console.WriteLine(userId);

            api.Logout(userId);
        }
    }
}
