using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP.Controller
{
    internal class Reports
    {
        SQLiteConnection connection;
        SQLiteCommand command;
        SQLiteDataAdapter dataAdapter;
        DataTable bufferTable;

        //Коструктор класс Reports
        public Reports(string conn)
        {
            connection = new SQLiteConnection(conn);
            connection.Open();
        }
    }
}
