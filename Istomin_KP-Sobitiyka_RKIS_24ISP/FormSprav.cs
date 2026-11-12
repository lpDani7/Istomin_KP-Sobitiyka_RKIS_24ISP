using Istomin_KP_Sobitiyka_RKIS_24ISP.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormSprav : Form
    {
        Query controller;

        string where;

        int mode = -1;

        public FormSprav(int tabIndex)
        {
            InitializeComponent();
            // [ИИ-блок|Начало]
            //Метод для добавления к каждой операции привязки данных метода сокрытия служебного столбца с первичным ключом
            dataGridViewResponsible.DataBindingComplete += (s, ev) => HideServiceColumnsSprav();
            dataGridViewPlaces.DataBindingComplete += (s, ev) => HideServiceColumnsSprav();
            dataGridViewTypesOfExpenses.DataBindingComplete += (s, ev) => HideServiceColumnsSprav();
            dataGridViewPreferentialCategories.DataBindingComplete += (s, ev) => HideServiceColumnsSprav();
            // [ИИ-блок|Конец]
            controller = new Query(AppSetting.ConnStr);
            //Заполнение таблиц-справочников записями
            dataGridViewResponsible.DataSource = controller.ResponsibleTable();
            dataGridViewPlaces.DataSource = controller.PlacesTable();
            dataGridViewTypesOfExpenses.DataSource = controller.TypesOfExpensesTable();
            dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable();
            //Открытие нужной страницы по выбранному в главной форме индексу
            tabControlMain.SelectedIndex = tabIndex;
            //Вывод количества записей
            ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
            ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces);
            ShowCountOfTableEntry(toolStripStatusLabelTypesOfExpenses, dataGridViewTypesOfExpenses);
            ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
            //Заполнения comboBox поиска значениями столбцов таблицы
            //FillingComboBoxSearch(comboBoxResponsibleSearch, dataGridViewResponsible);
            //FillingComboBoxSearch(comboBoxPlacesSearch, dataGridViewPlaces);
            //FillingComboBoxSearch(comboBoxTypesOfExpensesSearch, dataGridViewTypesOfExpenses);
            //FillingComboBoxSearch(comboBoxPreferentialCategoriesSearch, dataGridViewPreferentialCategories);
        }

        // [ИИ-блок|Начало]
        //Метод для сокрытия от пользователя служебного столбца с первичным ключом
        private void HideServiceColumnsSprav()
        {
            if (dataGridViewResponsible.Columns.Contains("Таб_номер"))
                dataGridViewResponsible.Columns["Таб_номер"].Visible = false;

            if (dataGridViewPlaces.Columns.Contains("Код_объекта"))
                dataGridViewPlaces.Columns["Код_объекта"].Visible = false;

            if (dataGridViewTypesOfExpenses.Columns.Contains("Код_записи"))
                dataGridViewTypesOfExpenses.Columns["Код_записи"].Visible = false;

            if (dataGridViewPreferentialCategories.Columns.Contains("Код_категории"))
                dataGridViewPreferentialCategories.Columns["Код_категории"].Visible = false;
        }
        // [ИИ-блок|Конец]


        private void FormSprav_Load(object sender, EventArgs e)
        {

        }

        /*//Метод заполнения comboBox поиска значениями столбцов таблицы
        private void FillingComboBoxSearch(System.Windows.Forms.ComboBox comboBoxSearch, DataGridView dataGridViewSearch)
        {
            comboBoxSearch.Items.Clear();
            foreach (DataGridViewColumn column in dataGridViewSearch.Columns)
            {
                comboBoxSearch.Items.Add(column.Name);
            }
        }*/

        //Метод вывода количества записей
        private void ShowCountOfTableEntry(ToolStripStatusLabel toolStripStatusLabel, DataGridView dataGridView)
        {
            toolStripStatusLabel.Text = $"Количество записей: {(dataGridView.RowCount).ToString()}";
        }

        //Функция поиска по первым буквам в Ответственных
        private void textBoxResponsibleSearch_TextChanged(object sender, EventArgs e)
        {
            where = $"";
            if (radioButtonResponsibleSearchName.Checked == true) where = $" WHERE ФИО LIKE '{textBoxResponsibleSearch.Text}%'";
            else if (radioButtonResponsibleSearchPost.Checked == true) where = $" WHERE Должность LIKE '{textBoxResponsibleSearch.Text}%'";
            dataGridViewResponsible.DataSource = controller.ResponsibleTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
        }

        //Функция поиска по первым буквам в Местах проведения
        private void textBoxPlacesSearch_TextChanged(object sender, EventArgs e)
        {
            where = $"";
            if (radioButtonPlaceSearchName.Checked == true) where = $" WHERE Название LIKE '{textBoxPlacesSearch.Text}%'";
            else if (radioButtonPlaceSearchAdress.Checked == true) where = $" WHERE Адрес LIKE '%{textBoxPlacesSearch.Text}%'";
            dataGridViewPlaces.DataSource = controller.PlacesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces);
        }

        //Функция поиска по первым буквам в Типах расходов
        private void textBoxTypesOfExpensesSearch_TextChanged(object sender, EventArgs e)
        {
            where = $" WHERE Тип_расхода LIKE '{textBoxTypesOfExpensesSearch.Text}%'";
            dataGridViewTypesOfExpenses.DataSource = controller.TypesOfExpensesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelTypesOfExpenses, dataGridViewTypesOfExpenses);
        }

        //Функция поиска по первым буквам в Льготных гатегориях
        private void textBoxPreferentialCategoriesSearch_TextChanged(object sender, EventArgs e)
        {
            where = $"";
            if (radioButtonPreferentialCategoriesSearchName.Checked == true) where = $" WHERE Название LIKE '{textBoxPreferentialCategoriesSearch.Text}%'";
            dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
        }

        //Метод поиска мест проведения по диапозону вместимости
        private void PlacesSearchCapacity()
        {
            where = $"";
            //Если textBox начала и конца не пустые выполняется запрос BETWEEN
            if (textBoxPlacesSearchCapacityUntil.Text != string.Empty && textBoxPlacesSearchCapacityFrom.Text != string.Empty)
                where = $" WHERE Вместимость BETWEEN {textBoxPlacesSearchCapacityFrom.Text} " +
                $"and {textBoxPlacesSearchCapacityUntil.Text}";
            dataGridViewPlaces.DataSource = controller.PlacesTable(where); //Заполнение таблицы значениями
            ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces); //Отображение количества записей
        }

        //Метод при изменении значения в textBox мест проведения
        private void textBoxPlacesSearchCapacityFrom_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPlacesSearchCapacity.Checked == true) PlacesSearchCapacity();
        }

        //Метод при изменении значения в textBox мест проведения
        private void textBoxPlacesSearchCapacityUntil_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPlacesSearchCapacity.Checked == true) PlacesSearchCapacity();
        }

        //Метод поиска льготных категорий по диапозону послаблений
        private void PreferentialCategoriesSearchConcessions()
        {
            where = $"";
            if (textBoxPreferentialCategoriesSearchConcessionsUntil.Text != string.Empty && textBoxPreferentialCategoriesSearchConcessionsFrom.Text != string.Empty)
                where = $" WHERE [Послабление(%)] BETWEEN {textBoxPreferentialCategoriesSearchConcessionsFrom.Text} " +
                $"and {textBoxPreferentialCategoriesSearchConcessionsUntil.Text}";
            dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
        }

        //Метод при изменении значения в textBox льготных категорий
        private void textBoxPreferentialCategoriesSearchConcessionsFrom_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPreferentialCategoriesSearchConcessions.Checked == true) PreferentialCategoriesSearchConcessions();
        }

        //Метод при изменении значения в textBox льготных категорий
        private void textBoxPreferentialCategoriesSearchConcessionsUntil_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPreferentialCategoriesSearchConcessions.Checked == true) PreferentialCategoriesSearchConcessions();
        }

        //Метод поиска ответственных мужчин
        private void radioButtonResponsibleSearchMan_CheckedChanged(object sender, EventArgs e)
        {
            where = $" WHERE Пол == 'Мужской'";
            dataGridViewResponsible.DataSource = controller.ResponsibleTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
        }

        //Метод поиска ответственных женщин
        private void radioButtonResponsibleSearchWoman_CheckedChanged(object sender, EventArgs e)
        {
            where = $" WHERE Пол == 'Женский'";
            dataGridViewResponsible.DataSource = controller.ResponsibleTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
        }

        //Метод поиска ответственных по диапозону дат рождения
        private void ResponsibleSearchDate()
        {
            where = $" WHERE Дата_рож BETWEEN '{dateTimePickerResponsibleSearchFrom.Value.ToString("yyyy-MM-dd")}' " +
                    $"and '{dateTimePickerResponsibleSearchUntil.Value.ToString("yyyy-MM-dd")}'";
            dataGridViewResponsible.DataSource = controller.ResponsibleTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
        }

        //Метод при изменении значения в dateTimePicker ответственных
        private void dateTimePickerResponsibleSearchFrom_ValueChanged(object sender, EventArgs e)
        {
            if (radioButtonResponsibleSearchDate.Checked == true) ResponsibleSearchDate();
        }

        //Метод при изменении значения в dateTimePicker ответственных
        private void dateTimePickerResponsibleSearchUntil_ValueChanged(object sender, EventArgs e)
        {
            if (radioButtonResponsibleSearchDate.Checked == true) ResponsibleSearchDate();
        }

        //Метод при нажатии поиска по дате RadioButton ответственных
        private void radioButtonResponsibleSearchDate_CheckedChanged(object sender, EventArgs e)
        {
            ResponsibleSearchDate();
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 5 символов
        private void textBoxPlacesSearchCapacityFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxPlacesSearchCapacityFrom.Text.Length > 5) e.KeyChar = (char)0;
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 5 символов
        private void textBoxPlacesSearchCapacityUntil_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxPlacesSearchCapacityUntil.Text.Length > 5) e.KeyChar = (char)0;
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 5 символов
        private void textBoxPreferentialCategoriesSearchConcessionsFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxPreferentialCategoriesSearchConcessionsFrom.Text.Length > 5) e.KeyChar = (char)0;
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 5 символов
        private void textBoxPreferentialCategoriesSearchConcessionsUntil_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxPreferentialCategoriesSearchConcessionsUntil.Text.Length > 5) e.KeyChar = (char)0;
        }

        //Функция выхода из окна справочники
        private void CloseFormSprave()
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из окна 'Справочники'", "Выход", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            else Close();
        }

        //Метод нажатия "Выход" на странице Ответственные
        private void выходToolStripMenuItemResponsible_Click(object sender, EventArgs e)
        {
            CloseFormSprave();
        }

        //Метод нажатия "Выход" на странице Места проведения
        private void выходToolStripMenuItemPlaces_Click(object sender, EventArgs e)
        {
            CloseFormSprave();
        }

        //Метод нажатия "Выход" на странице Типы расходов
        private void выходToolStripMenuItemTypesOfExpenses_Click(object sender, EventArgs e)
        {
            CloseFormSprave();
        }

        //Метод нажатия "Выход" на странице Льготные категории
        private void выходToolStripMenuItemPreferentialCategories_Click(object sender, EventArgs e)
        {
            CloseFormSprave();
        }

        //Метод открытия формы правки Ответсвтвенных
        private void OpenFormCorrectionResponsible(int mode)
        {
            FormCorrectionResponsible formCorrectionResponsible;
            if (mode == 1) formCorrectionResponsible = new FormCorrectionResponsible();
            else
            {
                DataGridViewRow row = dataGridViewResponsible.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Таб_номер"].Value);
                string name = row.Cells["ФИО"].Value.ToString();
                string post = row.Cells["Должность"].Value.ToString();
                string gender = row.Cells["Пол"].Value.ToString();
                string DateStr = row.Cells["Дата рождения"].Value.ToString();
                DateTime Date = DateTime.Parse(DateStr);
                formCorrectionResponsible = new FormCorrectionResponsible(id, name, post, Date, gender);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionResponsible.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewResponsible.DataSource = controller.ResponsibleTable();
                ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Метод открытия формы правки Мест проведения
        private void OpenFormCorrectionPlaces(int mode)
        {
            FormCorrectionPlaces formCorrectionPlaces;
            if (mode == 1) formCorrectionPlaces = new FormCorrectionPlaces();
            else
            {
                DataGridViewRow row = dataGridViewPlaces.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Код_объекта"].Value);
                string name = row.Cells["Название"].Value.ToString();
                string adress = row.Cells["Адрес"].Value.ToString();
                int capacity = Convert.ToInt32(row.Cells["Вместимость"].Value);
                formCorrectionPlaces = new FormCorrectionPlaces(id, name, adress, capacity);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionPlaces.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewPlaces.DataSource = controller.PlacesTable();
                ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces);
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Метод открытия формы правки Типов расходов
        private void OpenFormCorrectionTypesOfExpenses(int mode)
        {
            FormCorrectionTypesOfExpenses formCorrectionTypesOfExpenses;
            if (mode == 1) formCorrectionTypesOfExpenses = new FormCorrectionTypesOfExpenses();
            else
            {
                DataGridViewRow row = dataGridViewTypesOfExpenses.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Код_записи"].Value);
                string name = row.Cells["Тип расхода"].Value.ToString();
                formCorrectionTypesOfExpenses = new FormCorrectionTypesOfExpenses(id, name);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionTypesOfExpenses.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewTypesOfExpenses.DataSource = controller.TypesOfExpensesTable();
                ShowCountOfTableEntry(toolStripStatusLabelTypesOfExpenses, dataGridViewTypesOfExpenses);
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Метод открытия формы правки Льготных категорий
        private void OpenFormCorrectionPreferentialCategories(int mode)
        {
            FormCorrectionPreferentialCategories formCorrectionPreferentialCategories;
            if (mode == 1) formCorrectionPreferentialCategories = new FormCorrectionPreferentialCategories();
            else
            {
                DataGridViewRow row = dataGridViewPreferentialCategories.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Код_категории"].Value);
                string name = row.Cells["Название категории"].Value.ToString();
                int concession = Convert.ToInt32(row.Cells["Послабление(%)"].Value);
                formCorrectionPreferentialCategories = new FormCorrectionPreferentialCategories(id, name, concession);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionPreferentialCategories.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable();
                ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Метод открытия формы правки-добавления Ответсвтвенных
        private void toolStripMenuItemResponsibleAdd_Click(object sender, EventArgs e)
        {
            mode = 1;
            OpenFormCorrectionResponsible(mode);
        }

        //Метод открытия формы правки-редактирования Ответсвтвенных
        private void toolStripMenuItemResponsibleEdit_Click(object sender, EventArgs e)
        {
            mode = 2;
            OpenFormCorrectionResponsible(mode);
        }

        //Метод открытия формы правки-добавления Мест проведения
        private void toolStripMenuItemPlacesAdd_Click(object sender, EventArgs e)
        {
            mode = 1;
            OpenFormCorrectionPlaces(mode);
        }

        //Метод открытия формы правки-редактирования Мест проведения
        private void toolStripMenuItemPlacesEdit_Click(object sender, EventArgs e)
        {
            mode = 2;
            OpenFormCorrectionPlaces(mode);
        }

        //Метод открытия формы правки-добавления Типов расходов
        private void toolStripMenuItemTypesOfExpensesAdd_Click(object sender, EventArgs e)
        {
            mode = 1;
            OpenFormCorrectionTypesOfExpenses(mode);
        }

        //Метод открытия формы правки-редактирования Типов расходов
        private void toolStripMenuItemTypesOfExpensesEdit_Click(object sender, EventArgs e)
        {
            mode = 2;
            OpenFormCorrectionTypesOfExpenses(mode);
        }

        //Метод открытия формы правки-добавления Льготных категорий
        private void toolStripMenuItemPreferentialCategoriesAdd_Click(object sender, EventArgs e)
        {
            mode = 1;
            OpenFormCorrectionPreferentialCategories(mode);
        }

        //Метод открытия формы правки-редактирования Льготных категорий
        private void toolStripMenuItemPreferentialCategoriesEdit_Click(object sender, EventArgs e)
        {
            mode = 2;
            OpenFormCorrectionPreferentialCategories(mode);
        }

        //Метод удаления выделенной записи из Ответсвтвенных
        private void toolStripMenuItemResponsibleDelete_Click(object sender, EventArgs e)
        {
            mode = 3;
            DataGridViewRow row = dataGridViewResponsible.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Таб_номер"].Value);
            if (controller.IsResponsebleUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить ответственного " +
                                                            $"{dataGridViewResponsible.CurrentRow.Cells["ФИО"].Value}, " +
                                                            $"{dataGridViewResponsible.CurrentRow.Cells["Должность"].Value}?","Подтверждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes && controller.ModifyResponseble(mode, id, "", "", DateTime.Today, "")>0)
                {
                    MessageBox.Show($"Запись была удалена!");
                    dataGridViewResponsible.DataSource = controller.ResponsibleTable();
                    ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить ответстванного, так как он учавствует в организации мероприятия!", "Ошибка!");
                return;
            }
        }

        //Метод удаления выделенной записи из Мест проведения
        private void toolStripMenuItemPlacesDelete_Click(object sender, EventArgs e)
        {
            mode = 3;
            DataGridViewRow row = dataGridViewPlaces.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_объекта"].Value);
            if (controller.IsPlacesUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить место проведения " +
                                                            $"{dataGridViewPlaces.CurrentRow.Cells["Название"].Value}, " +
                                                            $"{dataGridViewPlaces.CurrentRow.Cells["Адрес"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes && controller.ModifyPlaces(mode, id, "", "", 0) > 0)
                {
                    MessageBox.Show($"Запись была удалена!");
                    dataGridViewPlaces.DataSource = controller.PlacesTable();
                    ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить место проведения, в нём организуется мероприятие!", "Ошибка!");
                return;
            }
        }

        //Метод удаления выделенной записи из Типов расходов
        private void toolStripMenuItemTypesOfExpensesDelete_Click(object sender, EventArgs e)
        {
            mode = 3;
            DataGridViewRow row = dataGridViewTypesOfExpenses.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_записи"].Value);
            if (controller.IsTypesOfExpensesUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить тип расхода " +
                                                            $"{dataGridViewTypesOfExpenses.CurrentRow.Cells["Тип расхода"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes && controller.ModifyTypesOfExpenses(mode, id, "") > 0)
                {
                    MessageBox.Show($"Запись была удалена!");
                    dataGridViewTypesOfExpenses.DataSource = controller.TypesOfExpensesTable();
                    ShowCountOfTableEntry(toolStripStatusLabelTypesOfExpenses, dataGridViewTypesOfExpenses);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить тип расхода, так как он учавствует в формировании сметы!", "Ошибка!");
                return;
            }
        }

        //Метод удаления выделенной записи из Льготных категорий
        private void toolStripMenuItemPreferentialCategoriesDelete_Click(object sender, EventArgs e)
        {
            mode = 3;
            DataGridViewRow row = dataGridViewPreferentialCategories.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_категории"].Value);
            if (controller.IsPreferentialCategoriesUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить место проведения " +
                                                            $"{dataGridViewPreferentialCategories.CurrentRow.Cells["Название категории"].Value}, " +
                                                            $"{dataGridViewPreferentialCategories.CurrentRow.Cells["Послабление(%)"].Value}%?", "Подтверждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes && controller.ModifyPreferentialCategories(mode, id, "", 0) > 0)
                {
                    MessageBox.Show($"Запись была удалена!");
                    dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable();
                    ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить льготную категорию, она используется в учёте льготной посещаемости!", "Ошибка!");
                return;
            }
        }

        //Событие обработки ввода текста в textBoxResponsibleSearch
        private void textBoxResponsibleSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) e.Handled = true; //Запрет на ввод цифр, сохранение ввода Backspace, Delete и Enter
            if (textBoxResponsibleSearch.Text.Length > 100) e.KeyChar = (char)0;
        }

        //Событие обработки ввода текста в textBoxCorrectionTypesOfExpensesName
        private void textBoxTypesOfExpensesSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) e.Handled = true; //Запрет на ввод цифр, сохранение ввода Backspace, Delete и Enter
            if (textBoxResponsibleSearch.Text.Length > 100) e.KeyChar = (char)0;
        }
    }
}
