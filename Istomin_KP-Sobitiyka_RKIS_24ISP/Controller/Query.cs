using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
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
            string querytovar = @"SELECT Таб_номер, ФИО, Должность, strftime('%d.%m.%Y', Дата_рож) AS 'Дата рождения', Пол
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
            string querytovar = @"SELECT Код_объекта, Название, Адрес, Вместимость 
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
            string querytovar = @"SELECT Код_записи, Тип_расхода AS 'Тип расхода'
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
            string querytovar = @"SELECT Код_категории, Название AS 'Название категории', [Послабление(%)]
                               FROM Льготные_категории";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция модификации данных Ответственных
        public int ModifyResponseble(int mode, int id, string name, string post, DateTime Date, string gender)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Ответственные (ФИО, Должность, Дата_рож, Пол) 
                          VALUES (@name, @post, @date, @gender)";
                    break;
                case 2:
                    sql = @"UPDATE Ответственные 
                          SET ФИО = @name, Должность = @post, Дата_рож = @date, Пол = @gender 
                          WHERE Таб_номер = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Ответственные WHERE Таб_номер = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3)
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@date", Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@post", post);
                command.Parameters.AddWithValue("@gender", gender);
            }
            return command.ExecuteNonQuery();
        }

        //Функция проверки на использование Ответственного в других таблицах БД
        public bool IsResponsebleUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Мероприятие WHERE Ответственный = @id";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0) return false;
                else return true;
            }
            catch { return false; }
        }

        //Функция прверки на наличие у Ответственного совпадений в БД
        public bool IsResponsibleHasCoincidence(string name, string post, string Date, string gender)
        {
            string sql = @"SELECT COUNT(*) FROM Ответственные WHERE ФИО = @name and Должность = @post and Дата_рож = @Date and Пол = @gender";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@post", post);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@gender", gender);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }
    }
}
