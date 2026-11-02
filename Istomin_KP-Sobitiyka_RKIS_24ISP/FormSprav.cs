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
            FillingComboBoxSearch(comboBoxResponsibleSearch, dataGridViewResponsible);
            FillingComboBoxSearch(comboBoxPlacesSearch, dataGridViewPlaces);
            FillingComboBoxSearch(comboBoxTypesOfExpensesSearch, dataGridViewTypesOfExpenses);
            FillingComboBoxSearch(comboBoxPreferentialCategoriesSearch, dataGridViewPreferentialCategories);
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
            where = $" WHERE {comboBoxResponsibleSearch.SelectedItem.ToString()} LIKE '{textBoxResponsibleSearch.Text}%'";
            dataGridViewResponsible.DataSource = controller.ResponsibleTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
        }

        //Функция поиска по первым буквам в Местах проведения
        private void textBoxPlacesSearch_TextChanged(object sender, EventArgs e)
        {
            where = $" WHERE {comboBoxPlacesSearch.SelectedItem.ToString()} LIKE '{textBoxPlacesSearch.Text}%'";
            dataGridViewPlaces.DataSource = controller.PlacesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelPlaces, dataGridViewPlaces);
        }

        //Функция поиска по первым буквам в Типах расходов
        private void textBoxTypesOfExpensesSearch_TextChanged(object sender, EventArgs e)
        {
            where = $" WHERE {comboBoxTypesOfExpensesSearch.SelectedItem.ToString()} LIKE '{textBoxTypesOfExpensesSearch.Text}%'";
            dataGridViewTypesOfExpenses.DataSource = controller.TypesOfExpensesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelTypesOfExpenses, dataGridViewTypesOfExpenses);
        }

        //Функция поиска по первым буквам в Льготных гатегориях
        private void textBoxPreferentialCategoriesSearch_TextChanged(object sender, EventArgs e)
        {
            where = $" WHERE {comboBoxPreferentialCategoriesSearch.SelectedItem.ToString()} LIKE '{textBoxPreferentialCategoriesSearch.Text}%'";
            dataGridViewPreferentialCategories.DataSource = controller.PreferentialCategoriesTable(where);
            ShowCountOfTableEntry(toolStripStatusLabelPreferentialCategories, dataGridViewPreferentialCategories);
        }

    }
}
