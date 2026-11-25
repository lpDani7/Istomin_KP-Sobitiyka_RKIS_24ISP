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

        //Функция получения количества мероприятий в месяц
        public DataTable CountEventTable(string having = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT strftime('%m', Дата) AS Месяц, count(Код_мероприятия) AS ""Количество мероприятий""
	                              FROM Мероприятия
	                              GROUP BY strftime('%Y', Дата), strftime('%m', Дата)";
            querytovar += having;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения не льготной, льготной и общей посещаемости в месяц
        public DataTable TrafficTable(string having = "")
        {
            bufferTable = new DataTable();
            string sql = @"SELECT strftime('%m', М.Дата) AS Месяц, SUM(М.Посещаемость) AS ""Посещаемость"", SUM(Л.[Льгот пос]) AS ""Льготная посещаемость"", SUM(М.Посещаемость) + SUM(Л.[Льгот пос]) AS ""Общая посещаемость""
	                              FROM Мероприятия М LEFT JOIN (
									                            SELECT Код_мероприятия, SUM(Количество) AS ""Льгот пос""
									                            FROM Льготная_посещаемость
									                            GROUP BY Код_мероприятия
									                             ) Л ON М.Код_мероприятия = Л.Код_мероприятия
	                              GROUP BY strftime('%m.%Y', М.Дата)";
            sql += having;
            dataAdapter = new SQLiteDataAdapter(sql, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения льготной посещаемости мероприятий в месяц по льготным категориям
        public DataTable TypeTrafficTable(string having = "")
        {
            bufferTable = new DataTable();
            string sql = @"SELECT К.Название AS ""Льготная категория"", SUM(К.Количество) AS ""Количество""
	                       FROM Мероприятия М LEFT JOIN (
									                     SELECT ЛП.Код_мероприятия, ЛК.Название, ЛП.Количество
									                     FROM Льготные_категории ЛК LEFT JOIN Льготная_посещаемость ЛП ON ЛК.Код_категории = ЛП.Код_категории
									                     GROUP BY ЛП.Код_мероприятия, ЛК.Название
									                      ) К ON М.Код_мероприятия = К.Код_мероприятия
	                       WHERE [Льготная категория] IS NOT NULL
	                       GROUP BY strftime('%m.%Y', М.Дата), К.Название";
            sql += having;
            dataAdapter = new SQLiteDataAdapter(sql, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения расходов на мероприятие в месяц
        public DataTable ExpensesTable(string having = "")
        {
            bufferTable = new DataTable();
            string sql = @"SELECT strftime('%m', М.Дата) AS Месяц, SUM(С.Расходы) AS ""Расход""
	                       FROM Мероприятия М LEFT JOIN (
									                     SELECT Код_мероприятия, SUM(Затраты) AS Расходы
									                     FROM Смета
									                     GROUP BY Код_мероприятия
									                      ) С ON М.Код_мероприятия = С.Код_мероприятия 
	                       GROUP BY strftime('%m.%Y', М.Дата)";
            sql += having;
            dataAdapter = new SQLiteDataAdapter(sql, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения доходов на мероприятие в месяц
        public DataTable IncomeTable(string having = "")
        {
            bufferTable = new DataTable();
            string sql = @"SELECT strftime('%m', М.Дата) AS Месяц, SUM(М.Цена_билета*М.Посещаемость)+SUM(П.[Людей при скидке]*М.Цена_билета) AS Доход
	                       FROM Мероприятия М LEFT JOIN (
									                     SELECT ЛП.Код_мероприятия, SUM(ЛК.[Послабление(%)]*1.0/100*ЛП.Количество) AS ""Людей при скидке"" 
									                     FROM Льготные_категории ЛК INNER JOIN Льготная_посещаемость ЛП ON ЛК.Код_категории = ЛП.Код_категории
									                     GROUP BY ЛП.Код_мероприятия
									                      ) П ON М.Код_мероприятия = П.Код_мероприятия
	                       GROUP BY strftime('%m.%Y', М.Дата)";
            sql += having;
            dataAdapter = new SQLiteDataAdapter(sql, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех годов мероприятий
        public DataTable YearTable(string having = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT DISTINCT strftime('%Y', М.Дата) AS Год
	                              FROM Мероприятия М";
            querytovar += having;
            querytovar += " ORDER BY М.Дата";
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех месяцев мероприятияй мероприятий за год
        public DataTable MonthTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT DISTINCT strftime('%m', М.Дата) AS Месяц
	                              FROM Мероприятия М";
            querytovar += where;
            querytovar += " ORDER BY М.Дата";
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }
    }
}
