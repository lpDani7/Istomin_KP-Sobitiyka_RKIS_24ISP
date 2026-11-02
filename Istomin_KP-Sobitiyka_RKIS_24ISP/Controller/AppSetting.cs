using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP.Controller
{
    internal class AppSetting
    {
        public static string ConnStr
        {
            get
            {
                return "Data Source=Sobitiyka.db;Version=3";
            }
        }
    }
}
