using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP.Controller
{
    internal class Query
    {
        SQLiteConnection connection;
        SQLiteCommand command;
        SQLiteDataAdapter dataAdapter;
        DataTable bufferTable;

        //Коструктор класс Query
        public Query(string conn)
        {
            connection = new SQLiteConnection(conn);
            connection.Open();
        }
        //Запрос получения всех Ответственных
        static string responsibleTable = @"SELECT * FROM Ответственные ORDER BY ФИО";
        //Запрос получения всех Мест проведения
        static string placesTable = @"SELECT * FROM Места_проведения ORDER BY Название";
        //Запрос получения всех Типов расходов
        static string typesOfExpensesTable = @"SELECT * FROM Типы_расходов ORDER BY Тип_расхода";
        //Запрос получения всех Льготных категорий
        static string preferentialCategoriesTable = @"SELECT * FROM Льготные_категории ORDER BY Название";

        //Функция получения всех Ответственных
        public DataTable ResponsibleTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT *
                               FROM Ответственные";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех Мест проведения
        public DataTable PlacesTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT *
                               FROM Места_проведения";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех Типов расходов
        public DataTable TypesOfExpensesTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT *
                               FROM Типы_расходов";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех Льготных категорий
        public DataTable PreferentialCategoriesTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT *
                               FROM Льготные_категории";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }
    }
}
