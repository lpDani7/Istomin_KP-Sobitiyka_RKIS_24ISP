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

        private void FormSprav_Load(object sender, EventArgs e)
        {

        }

        /*//Метод заполнения таблицы-справочника записями
        private void FillingTables(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = controller.dataTable;
        }*/

        /*//Метод поиска по первым буквам
        private void SearchEntryOnLetter(ToolStripStatusLabel toolStripStatusLabel, DataGridView dataGridView)
        {
            string where = "";
            where = $" WHERE {comboBox1.SelectedItem.ToString()} LIKE '{textBox1.Text}%'";
            OrderDataGrid.DataSource = controller.TotalOrders(where);
            ShowCountOfTableEntry(toolStripStatusLabel, dataGridView);
        }*/

        //Метод заполнения comboBox поиска значениями столбцов таблицы
        private void FillingComboBoxSearch(System.Windows.Forms.ComboBox comboBoxSearch, DataGridView dataGridViewSearch)
        {
            comboBoxSearch.Items.Clear();
            foreach (DataGridViewColumn column in dataGridViewSearch.Columns)
            {
                comboBoxSearch.Items.Add(column.Name);
            }
        }

        //Метод вывода количества записей
        private void ShowCountOfTableEntry(ToolStripStatusLabel toolStripStatusLabel, DataGridView dataGridView)
        {
            toolStripStatusLabel.Text = $"Количество записей: {(dataGridView.RowCount - 1).ToString()}";
        }

        //Функция поиска по первым буквам в Ответственных
        private void textBoxResponsibleSearch_TextChanged(object sender, EventArgs e)
        {
            where = $"";
            if (radioButtonResponsibleSearchName .Checked == true) where = $" WHERE ФИО LIKE '{textBoxResponsibleSearch.Text}%'";
            else if (radioButtonResponsibleSearchPost .Checked == true) where = $" WHERE Должность LIKE '{textBoxResponsibleSearch.Text}%'";
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
            //Если textBox начала и конца не пустые выполняется запрос BETWEEN
            if (textBoxPlacesSearchCapacityUntil.Text != string.Empty && textBoxPlacesSearchCapacityFrom.Text != string.Empty) 
                                                                               where = $" WHERE Вместимость BETWEEN {textBoxPlacesSearchCapacityFrom.Text} " +
                                                                               $"and {textBoxPlacesSearchCapacityUntil.Text}";
            dataGridViewPlaces.DataSource = controller.PlacesTable(where); //Заполнение таблицы значениями
            ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces); //Отображение количества записей
        }

        //Метод при изменении значения в textBox
        private void textBoxPlacesSearchCapacityFrom_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPlacesSearchCapacity.Checked == true) PlacesSearchCapacity();
        }

        //Метод при изменении значения в textBox
        private void textBoxPlacesSearchCapacityUntil_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPlacesSearchCapacity.Checked == true) PlacesSearchCapacity();
        }

        //Метод поиска льготных категорий по диапозону послаблений
        private void PreferentialCategoriesSearchConcessions()
        {
            if (textBoxPreferentialCategoriesSearchConcessionsUntil.Text != string.Empty && textBoxPreferentialCategoriesSearchConcessionsFrom.Text != string.Empty) 
                                                                               where = $" WHERE [Послабление(%)] BETWEEN {textBoxPreferentialCategoriesSearchConcessionsFrom.Text} " +
                                                                               $"and {textBoxPreferentialCategoriesSearchConcessionsUntil.Text}";
            dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
        }

        //Метод при изменении значения в textBox
        private void textBoxPreferentialCategoriesSearchConcessionsFrom_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonPreferentialCategoriesSearchConcessions.Checked == true) PreferentialCategoriesSearchConcessions();
        }

        //Метод при изменении значения в textBox
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

        //Метод при изменении значения в dateTimePicker
        private void dateTimePickerResponsibleSearchFrom_ValueChanged(object sender, EventArgs e)
        {
            if (radioButtonResponsibleSearchDate.Checked == true) ResponsibleSearchDate();
        }

        //Метод при изменении значения в dateTimePicker
        private void dateTimePickerResponsibleSearchUntil_ValueChanged(object sender, EventArgs e)
        {
            if (radioButtonResponsibleSearchDate.Checked == true) ResponsibleSearchDate();
        }

        //Метод при назажии RadioButton
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

        private void OpenFormCorrectionResponsible()
        {
            FormCorrectionResponsible formCorrectionResponsible = new FormCorrectionResponsible();
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formCorrectionResponsible.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        private void OpenFormCorrectionPlaces()
        {
            FormCorrectionPlaces formCorrectionPlaces = new FormCorrectionPlaces();
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formCorrectionPlaces.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        private void OpenFormCorrectionTypesOfExpenses()
        {
            FormCorrectionTypesOfExpenses formCorrectionTypesOfExpenses = new FormCorrectionTypesOfExpenses();
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formCorrectionTypesOfExpenses.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        private void OpenFormCorrectionPreferentialCategories()
        {
            FormCorrectionPreferentialCategories formCorrectionPreferentialCategories = new FormCorrectionPreferentialCategories();
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formCorrectionPreferentialCategories.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        private void toolStripMenuItemResponsibleAdd_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionResponsible();
        }

        private void toolStripMenuItemResponsibleEdit_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionResponsible();
        }

        private void toolStripMenuItemPlacesAdd_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionPlaces();
        }

        private void toolStripMenuItemPlacesEdit_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionPlaces();
        }

        private void toolStripMenuItemTypesOfExpensesAdd_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionTypesOfExpenses();
        }

        private void toolStripMenuItemTypesOfExpensesEdit_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionTypesOfExpenses();
        }

        private void toolStripMenuItemPreferentialCategoriesAdd_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionPreferentialCategories();
        }

        private void toolStripMenuItemPreferentialCategoriesEdit_Click(object sender, EventArgs e)
        {
            OpenFormCorrectionPreferentialCategories();
        }
    }
}
