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

        //Функция получения всех Мероприятий
        public DataTable EventsTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT Мер.Код_мероприятия, Мер.Название, strftime('%d.%m.%Y', Мер.Дата) AS 'Дата проведения', 
                                Мест.Название AS 'Место проведения', Мер.Цена_билета AS 'Цена билета', Мер.Тематика, Мер.Посещаемость AS 'Не льготная посещаемость' 
                                FROM Мероприятия Мер INNER JOIN Места_проведения Мест ON Мер.Место_проведения = Мест.Код_объекта
                                ";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех Исполнителей
        public DataTable EventPerformersTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT К.Название
                                FROM Коллективы К INNER JOIN Исполнители И ON К.Код_коллектива = И.Код_коллектива";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех Ответственных за мероприятие
        public DataTable ResponsibleForEventTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT О.ФИО, О.Должность, О.Дата_рож AS 'Дата рождения', О.Пол  
                                FROM Ответственные О INNER JOIN Ответственные_за_мероприятие ОМ ON О.Таб_номер = ОМ.Таб_номер";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех Расходов на мероприятие
        public DataTable ExpensesForEventTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT Т.Тип_расхода AS 'Тип расхода', С.Затраты, С.Описание 
                                FROM Типы_расходов Т INNER JOIN Смета С ON С.Тип_расхода = Т.Код_записи";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всей Льготной посещаемости мероприятия
        public DataTable PreferentialCategoriesAtEventTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT ЛК.Название, ЛК.[Послабление(%)], ЛП.Количество  
                                FROM Льготные_категории ЛК INNER JOIN Льготная_посещаемость ЛП ON ЛК.Код_категории = ЛП.Код_категории";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения суммы затрат на выбранное мероприятие
        public int ExpensesSum(int id)
        {
            string sql = @"SELECT SUM(Затраты)
                               FROM Смета 
                               WHERE Код_мероприятия = @id
                               GROUP BY Код_мероприятия";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            int sum = Convert.ToInt32(command.ExecuteScalar());
            return sum;
        }

        //Функция получения суммы льготной посещвемости на выбранном мероприятии
        public int PreferentialTrafficSum(int id)
        {
            string sql = @"SELECT SUM(Количество)
                               FROM Льготная_посещаемость 
                               WHERE Код_мероприятия = @id
                               GROUP BY Код_мероприятия";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            int sum = Convert.ToInt32(command.ExecuteScalar());
            return sum;
        }

        //Функция получения не льготной посещвемости на выбранном мероприятии
        public int NotPreferentialTrafficSum(int id)
        {
            string sql = @"SELECT Посещаемость
                               FROM Мероприятия 
                               WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            int sum = Convert.ToInt32(command.ExecuteScalar());
            return sum;
        }

        //Функция получения названия выбранного мероприятия
        public string SelectedEventName(int id)
        {
            string sql = @"SELECT Название
                               FROM Мероприятия 
                               WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            string name = command.ExecuteScalar().ToString();
            return name;
        }

        //Функция получения названия выбранного мероприятия
        public string SelectedEventDate(int id)
        {
            string sql = @"SELECT strftime('%d.%m.%Y', Дата) AS Дата
                               FROM Мероприятия 
                               WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            string name = command.ExecuteScalar().ToString();
            return name;
        }

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

        //Функция модификации данных Мест проведения
        public int ModifyPlaces(int mode, int id, string name, string adress, int capacity)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Места_проведения (Адрес, Название, Вместимость) 
                          VALUES (@adress, @name, @capacity)";
                    break;
                case 2:
                    sql = @"UPDATE Места_проведения 
                          SET Адрес = @adress, Название = @name, Вместимость = @capacity 
                          WHERE Код_объекта = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Места_проведения WHERE Код_объекта = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3)
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@adress", adress);
                command.Parameters.AddWithValue("@capacity", capacity);
            }
            return command.ExecuteNonQuery();
        }

        //Функция модификации данных Типов расходов
        public int ModifyTypesOfExpenses(int mode, int id, string name)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Типы_расходов (Тип_расхода) 
                          VALUES (@name)";
                    break;
                case 2:
                    sql = @"UPDATE Типы_расходов 
                          SET Тип_расхода = @name
                          WHERE Код_записи = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Типы_расходов WHERE Код_записи = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3)
            {
                command.Parameters.AddWithValue("@name", name);
            }
            return command.ExecuteNonQuery();
        }

        //Функция модификации данных Льготных категорий
        public int ModifyPreferentialCategories(int mode, int id, string name, int concession)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Льготные_категории (Название, [Послабление(%)]) 
                          VALUES (@name, @concession)";
                    break;
                case 2:
                    sql = @"UPDATE Льготные_категории 
                          SET Название = @name, [Послабление(%)] = @concession 
                          WHERE Код_категории = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Льготные_категории WHERE Код_категории = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3)
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@concession", concession);
            }
            return command.ExecuteNonQuery();
        }

        //Функция проверки на использование Ответственного в других таблицах БД
        public bool IsResponsebleUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Ответственный_за_мероприятия WHERE Таб_номер = @id";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0) return false;
                else return true;
            }
            catch { return false; }
        }

        //Функция проверки на использование Места проведения в других таблицах БД
        public bool IsPlacesUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Мероприятия WHERE Место_проведения = @id";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0) return false;
                else return true;
            }
            catch { return false; }
        }

        //Функция проверки на использование Типов расходов в других таблицах БД
        public bool IsTypesOfExpensesUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Смета WHERE Тип_расхода = @id";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0) return false;
                else return true;
            }
            catch { return false; }
        }

        //Функция проверки на использование Льготных категорий в других таблицах БД
        public bool IsPreferentialCategoriesUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Льготная_посещаемость WHERE Код_категории = @id";
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

        //Функция прверки на наличие у Места проведения совпадений в БД
        public bool IsPlacesHasCoincidence(string name, string adress, int capacity)
        {
            string sql = @"SELECT COUNT(*) FROM Места_проведения WHERE Название = @name and Адрес = @adress and Вместимость = @capacity";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@adress", adress);
            command.Parameters.AddWithValue("@capacity", capacity);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция прверки на наличие у Типов расходов совпадений в БД
        public bool IsTypesOfExpensesHasCoincidence(string name)
        {
            string sql = @"SELECT COUNT(*) FROM Типы_расходов WHERE Тип_расхода = @name";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция прверки на наличие у Льготных категорий совпадений в БД
        public bool IsPreferentialCategoriesHasCoincidence(string name, int concession)
        {
            string sql = @"SELECT COUNT(*) FROM Льготные_категории WHERE Название = @name and [Послабление(%)] = @concession";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@concession", concession);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }
    }
}
