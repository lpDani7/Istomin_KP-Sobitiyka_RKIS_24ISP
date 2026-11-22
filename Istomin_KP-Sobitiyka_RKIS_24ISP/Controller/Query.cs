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
                                Мест.Название AS 'Место проведения', Мер.Цена_билета AS 'Цена билета', Мер.Тематика, Мер.Посещаемость AS 'Не льготная посещаемость',
                                Мест.Код_объекта
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
            string querytovar = @"SELECT К.Название, И.Код_записи
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
            string querytovar = @"SELECT О.ФИО, О.Должность, О.Дата_рож AS 'Дата рождения', О.Пол, ОМ.Код_записи, ОМ.Таб_номер  
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
            string querytovar = @"SELECT Т.Тип_расхода AS 'Тип расхода', С.Затраты, С.Описание, С.Номер_записи 
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
            string querytovar = @"SELECT ЛК.Название, ЛК.[Послабление(%)], ЛП.Количество, ЛП.Номер_записи  
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

        //Функция получения всех Коллективов
        public DataTable CollectivesTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT Код_коллектива, Название AS 'Название коллектива'
                               FROM Коллективы";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех участников выбранного коллектива
        public DataTable CompositionOfCollectivesTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT СК.Код_участника, СК.ФИО, СК.Должность, strftime('%d.%m.%Y', СК.Дата_рож) AS 'Дата рождения', СК.Пол, СК.Код_коллектива
                                FROM Состав_коллектива СК ";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех кодов коллективов в участниках и названий этих коллективов
        public DataTable CompositionIDAndCollectivesNameTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT К.Название, К.Код_коллектива
                                FROM Коллективы К";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        /*//Функция получения всех свободных участников коллективов
        public DataTable FreeCompositionTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT СК.Код_участника, СК.ФИО, СК.Должность, strftime('%d.%m.%Y', СК.Дата_рож) AS 'Дата рождения', СК.Пол, СК.Код_коллектива
                                FROM Состав_коллектива СК
                                WHERE СК.Код_коллектива IS NULL";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }*/

        //Функция модификации данных Коллективов
        public int ModifyCollectives(int mode, int id, string name)
        {
            string sql = "";
            string dop = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Коллективы (Название) 
                          VALUES (@name)";
                    break;
                case 2:
                    sql = @"UPDATE Коллективы 
                          SET Название = @name
                          WHERE Код_коллектива = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Коллективы WHERE Код_коллектива = @id";
                    dop = @"UPDATE Состав_коллектива 
                          SET Код_коллектива = NULL
                          WHERE Код_коллектива = @id";
                    break;
                case 4:
                    sql = @"DELETE FROM Коллективы WHERE Код_коллектива = @id";
                    dop = @"DELETE FROM Состав_коллектива WHERE Код_коллектива = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3 && mode != 4)
            {
                command.Parameters.AddWithValue("@name", name);
            }
            command.ExecuteNonQuery();
            command = new SQLiteCommand(dop, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3 && mode!= 4)
            {
                command.Parameters.AddWithValue("@name", name);
            }
            return command.ExecuteNonQuery();
        }

        //Функция прверки на наличие у Коллектива совпадений в БД
        public bool IsCollectivesHasCoincidence(string name)
        {
            string sql = @"SELECT COUNT(*) FROM Коллективы WHERE Название = @name";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция получения номера коллектива по названию
        public int IDCollectivesByName(string name)
        {
            string sql = @"SELECT Код_коллектива
                               FROM Коллективы 
                               WHERE Название = @name";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            int id = Convert.ToInt32(command.ExecuteScalar());
            return id;
        }

        //Функция обновления Кода_коллектива у Участника
        public int ModifyCompositionIDCollectives(int mode, int idCom, int idCol)
        {
            string sql = "";
            if (mode == 1)
            {
                sql = @"UPDATE Состав_коллектива 
                           SET Код_коллектива = @idCol
                           WHERE Код_участника = @idCom";
            }
            else if (mode == 2)
            {
                sql = @"UPDATE Состав_коллектива 
                           SET Код_коллектива = NULL
                           WHERE Код_участника = @idCom";
            }
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@idCol", idCol);
            command.Parameters.AddWithValue("@idCom", idCom);
            return command.ExecuteNonQuery();
        }

        //Функция проверки на использование Коллектива в других таблицах БД
        public bool IsCollectivesUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Исполнители WHERE Код_коллектива = @id";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0) return false;
                else return true;
            }
            catch { return false; }
        }

        //Функция прверки на наличие у Участника совпадений в БД для добавления
        public bool IsCompositionHasCoincidenceForAdd(string name, string post, string Date, string gender)
        {
            string sql = @"SELECT COUNT(*) FROM Состав_коллектива WHERE ФИО = @name and Должность = @post and Дата_рож = @Date and Пол = @gender";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@post", post);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@gender", gender);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция прверки на наличие у Участника совпадений в БД для редактирования
        public bool IsCompositionHasCoincidenceForEdit(string name, string post, string Date, string gender, int collective)
        {
            string sql = @"SELECT COUNT(*) FROM Состав_коллектива WHERE ФИО = @name and Должность = @post and Дата_рож = @Date 
                           and Пол = @gender and Код_коллектива = @collective";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@post", post);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@collective", collective);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция модификации данных Участников
        public int ModifyComposition(int mode, int id, string name, string post, DateTime Date, string gender, int collective, bool chek)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    if (chek) sql = @"INSERT INTO Состав_коллектива (ФИО, Должность, Дата_рож, Пол, Код_коллектива) 
                              VALUES (@name, @post, @date, @gender, NULL)";
                    else sql = @"INSERT INTO Состав_коллектива (ФИО, Должность, Дата_рож, Пол, Код_коллектива) 
                              VALUES (@name, @post, @date, @gender, @collective)";
                    break;
                case 2:
                    if (chek) sql = @"UPDATE Состав_коллектива 
                          SET ФИО = @name, Должность = @post, Дата_рож = @date, Пол = @gender, Код_коллектива = NULL
                          WHERE Код_участника = @id";
                    else sql = @"UPDATE Состав_коллектива 
                          SET ФИО = @name, Должность = @post, Дата_рож = @date, Пол = @gender, Код_коллектива = @collective
                          WHERE Код_участника = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Состав_коллектива WHERE Код_участника = @id";
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
                command.Parameters.AddWithValue("@collective", collective);
            }
            return command.ExecuteNonQuery();
        }

        //Функция проверки на использование Участника в коллективе
        public bool IsCompositionUsed(int id)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Состав_коллектива WHERE Код_участника = @id and Код_коллектива IS NOT NULL";
                command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0) return false;
                else return true;
            }
            catch { return false; }
        }

        //Функция получения всех кодов мест проведения в мероприятиях и названий этих мест проведения
        public DataTable PlacesIDAndNameTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT Название, Код_объекта
                                FROM Места_проведения";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция прверки на наличие у Мероприятия совпадений в БД
        public bool IsEventHasCoincidence(string name, string Date, string topic, int placeID, int price, int traffic)
        {
            string sql = @"SELECT COUNT(*) FROM Мероприятия WHERE Название = @name and Дата = @Date and 
                           Тематика = @topic and Место_проведения = @placeID and Цена_билета = @price and Посещаемость = @traffic";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@topic", topic);
            command.Parameters.AddWithValue("@placeID", placeID);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@traffic", traffic);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция модификации данных Мероприятия
        public int ModifyEvent(int mode, int id, string name, string Date, string topic, int placeID, int price, int traffic)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Мероприятия (Название, Дата, Тематика, Место_проведения, Цена_билета, Посещаемость) 
                          VALUES (@name, @Date, @topic, @placeID, @price, @traffic)";
                    break;
                case 2:
                    sql = @"UPDATE Мероприятия 
                          SET Название = @name, Дата = @Date, Тематика = @topic, Место_проведения = @placeID, 
                          Цена_билета = @price, Посещаемость = @traffic
                          WHERE Код_мероприятия = @id";
                    break;
                case 3:
                    sql = @"DELETE FROM Мероприятия WHERE Код_мероприятия = @id";
                    //sql = @"DELETE М, И FROM Мероприятия М INNER JOIN Исполнители И ON М.Код_мероприятия = И.Код_мероприятия WHERE М.Код_мероприятия = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            if (mode != 1) command.Parameters.AddWithValue("@id", id);
            if (mode != 3)
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@Date", Date);
                command.Parameters.AddWithValue("@topic", topic);
                command.Parameters.AddWithValue("@placeID", placeID);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@traffic", traffic);
            }
            return command.ExecuteNonQuery();
        }

        //Функция удаления всей привязанной к мероприятию информации
        public void DeleteEventConnection(int id)
        {
            string sql = "";
            sql = @"DELETE FROM Исполнители WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            sql = @"DELETE FROM Смета WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            sql = @"DELETE FROM Ответственные_за_мероприятие WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            sql = @"DELETE FROM Льготная_посещаемость WHERE Код_мероприятия = @id";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            //command.ExecuteNonQuery();
        }

        //Функция получения номера мероприятия по его данным
        public int IDEventByData(string name, string Date, string topic, int placeID, int price, int traffic)
        {
            string sql = @"SELECT Код_мероприятия
                               FROM Мероприятия 
                               WHERE Название = @name and Дата = @Date and Тематика = @topic and Место_проведения = @placeID and 
                               Цена_билета = @price and Посещаемость = @traffic";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@topic", topic);
            command.Parameters.AddWithValue("@placeID", placeID);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@traffic", traffic);
            int id = Convert.ToInt32(command.ExecuteScalar());
            return id;
        }

        //Функция получения всех кодов льготных категорий, названий этих льготных категорий и их процент послабления
        public DataTable PreferentialCategoriesIDAndNameAndPercentTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT Код_категории, Название || ' - ' || [Послабление(%)] || '%' AS Название_Послабление
                                FROM Льготные_категории";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех кодов коллективов не учавствующих в огранизации данного мероприятия и названий этих коллективов
        public DataTable CollectivesIDAndNameTable(int idEvent)
        {
            bufferTable = new DataTable();
            string querytovar = $@"SELECT Название, Код_коллектива
                                FROM Коллективы
                                WHERE Код_коллектива NOT IN (SELECT Код_коллектива
                                                             FROM Исполнители
                                                             WHERE Код_мероприятия = {idEvent})";
            //querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция получения всех кодов типов расходов и названий этих типов расходов
        public DataTable TypesOfExpensesIDAndNameTable(string where = "")
        {
            bufferTable = new DataTable();
            string querytovar = @"SELECT Тип_расхода, Код_записи
                                FROM Типы_расходов";
            querytovar += where;
            dataAdapter = new SQLiteDataAdapter(querytovar, connection);
            dataAdapter.Fill(bufferTable);
            return bufferTable;
        }

        //Функция модификации данных Льготной посещаемости
        public int ModifyPreferentialAttendance(int mode, int idEvent, int id, int category, int count)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Льготная_посещаемость (Код_мероприятия, Код_категории, Количество) 
                          VALUES (@idEvent, @category, @count)";
                    break;
                case 2:
                    sql = @"UPDATE Льготная_посещаемость 
                          SET Количество = Количество+@count
                          WHERE Код_мероприятия = @idEvent and Код_категории = @category";
                    break;
                case 3:
                    sql = @"DELETE FROM Льготная_посещаемость WHERE Номер_записи = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idEvent", idEvent);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@count", count);
            return command.ExecuteNonQuery();
        }

        //Функция прверки на наличие у Льготной посещаемости совпадений в БД
        public bool IsPreferentialAttendanceHasCoincidence(int idEvent, int category)
        {
            string sql = @"SELECT COUNT(*) FROM Льготная_посещаемость WHERE Код_мероприятия = @idEvent and Код_категории = @category";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@idEvent", idEvent);
            command.Parameters.AddWithValue("@category", category);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция модификации данных Исполнителей
        public int ModifyPerformers(int mode, int idEvent, int id, int collectives)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Исполнители  (Код_мероприятия, Код_коллектива) 
                          VALUES (@idEvent, @collectives)";
                    break;
                case 2:
                    sql = @"DELETE FROM Исполнители WHERE Код_записи = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idEvent", idEvent);
            command.Parameters.AddWithValue("@collectives", collectives);
            return command.ExecuteNonQuery();
        }

        //Функция модификации данных Сметы
        public int ModifyExpensesForEvent(int mode, int idEvent, int id, int type, int expenses, string description)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Смета (Код_мероприятия, Тип_расхода, Затраты, Описание) 
                          VALUES (@idEvent, @type, @expenses, @description)";
                    break;
                case 2:
                    sql = @"UPDATE Смета 
                          SET Затраты = Затраты+@expenses
                          WHERE Код_мероприятия = @idEvent and Тип_расхода = @type and Описание = @description";
                    break;
                case 3:
                    sql = @"DELETE FROM Смета WHERE Номер_записи = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idEvent", idEvent);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@expenses", expenses);
            command.Parameters.AddWithValue("@description", description);
            return command.ExecuteNonQuery();
        }

        //Функция прверки на наличие у Сметы совпадений в БД
        public bool IsExpensesForEventHasCoincidence(int idEvent, int type, string description)
        {
            string sql = @"SELECT COUNT(*) FROM Смета WHERE Код_мероприятия = @idEvent and Тип_расхода = @type and Описание = @description";
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@idEvent", idEvent);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@description", description);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0) return false;
            else return true;
        }

        //Функция модификации данных Ответственные за мероприятие
        public int ModifyResponsibleForEvent(int mode, int idEvent, int id, int responsible)
        {
            string sql = "";
            switch (mode)
            {
                case 1:
                    sql = @"INSERT INTO Ответственные_за_мероприятие (Код_мероприятия, Таб_номер) 
                          VALUES (@idEvent, @responsible)";
                    break;
                case 2:
                    sql = @"DELETE FROM Ответственные_за_мероприятие WHERE Код_записи = @id";
                    break;
                default: break;
            }
            command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idEvent", idEvent);
            command.Parameters.AddWithValue("@responsible", responsible);
            return command.ExecuteNonQuery();
        }
    }
}
