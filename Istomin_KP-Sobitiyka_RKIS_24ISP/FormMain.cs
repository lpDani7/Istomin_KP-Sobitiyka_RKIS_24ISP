using Istomin_KP_Sobitiyka_RKIS_24ISP.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormMain : Form
    {
        Query controller;

        List<string> eventWhere = new List<string>();

        public FormMain()
        {
            InitializeComponent();
            //Метод для добавления к каждой операции привязки данных метода сокрытия служебного столбца с первичным ключом
            dataGridViewEvents.DataBindingComplete += (s, ev) => HideServiceColumnsMain();
            controller = new Query(AppSetting.ConnStr);
            dataGridViewEvents.DataSource = controller.EventsTable();
            ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
        }

        //Метод для сокрытия от пользователя служебного столбца с первичным ключом
        private void HideServiceColumnsMain()
        {
            if (dataGridViewEvents.Columns.Contains("Код_мероприятия"))
                dataGridViewEvents.Columns["Код_мероприятия"].Visible = false;

            /*if (dataGridViewPlaces.Columns.Contains("Код_объекта"))
                dataGridViewPlaces.Columns["Код_объекта"].Visible = false;

            if (dataGridViewTypesOfExpenses.Columns.Contains("Код_записи"))
                dataGridViewTypesOfExpenses.Columns["Код_записи"].Visible = false;

            if (dataGridViewPreferentialCategories.Columns.Contains("Код_категории"))
                dataGridViewPreferentialCategories.Columns["Код_категории"].Visible = false;*/
        }

        //Метод вывода количества записей
        private void ShowCountOfMainTableEntry(Label label, DataGridView dataGridView)
        {
            label.Text = $"{(dataGridView.RowCount).ToString()}";
        }

        //Метод открытия формы Справочника на странице Ответственные
        private void ответственныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(0);
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formSprav.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;

        }

        //Метод открытия формы Справочника на странице Места проведения
        private void местаПроведенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(1);
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formSprav.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Метод открытия формы Справочника на странице Типы расходов
        private void типыРасходовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(2);
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formSprav.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Метод открытия формы Справочника на странице Льготные категории
        private void льготныеКатегорииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(3);
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            // Открываем вторую форму модально
            formSprav.ShowDialog(this); // Передача this означает, что исходной форме передаётся фокус
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Функция выхода из приложения
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            else Close();
        }

        //Функция открытия формы коллективов
        private void коллективыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollectives collection = new FormCollectives();
            collection.ShowDialog(this);
        }

        //Функция обновления вспомогательных таблиц при нажатии на строку главной таблицы
        private void dataGridViewEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridViewEvents.CurrentRow;
            // Читаем значения из ячеек выбранной строки
            int id = Convert.ToInt32(row.Cells["Код_мероприятия"].Value);

            //Заполнение вспомогательных таблиц данными, на основе выбранной строки в главной таблице
            string where = $" WHERE И.Код_мероприятия = {id}";
            dataGridViewEventPerformers.DataSource = controller.EventPerformersTable(where);
            where = $" WHERE ОМ.Код_мероприятия = {id}";
            dataGridViewResponsibleForEvent.DataSource = controller.ResponsibleForEventTable(where);
            where = $" WHERE С.Код_мероприятия = {id}";
            dataGridViewExpensesForEvent.DataSource = controller.ExpensesForEventTable(where);
            where = $" WHERE ЛП.Код_мероприятия = {id}";
            dataGridViewPreferentialCategoriesAtEvent.DataSource = controller.PreferentialCategoriesAtEventTable(where);

            //Заполнение label таблиц дополнительной информации
            ShowCountOfMainTableEntry(labelEventResponsibleCount, dataGridViewResponsibleForEvent);
            ShowCountOfMainTableEntry(labelEventPerformersCount, dataGridViewEventPerformers);
            labelEventExpensesSum.Text = controller.ExpensesSum(id).ToString();
            labelEventPreferentialTrafficSum.Text = controller.PreferentialTrafficSum(id).ToString();
            labelEventGeneralTrafficSum.Text = (controller.NotPreferentialTrafficSum(id) + controller.PreferentialTrafficSum(id)).ToString();
            labelEventSelectedNameAndDate.Text = (controller.SelectedEventName(id) + "  " + controller.SelectedEventDate(id)).ToString();
        }

        //Функция нажатия на кнопку "Сбросить" для сброса всех значений фильтра
        private void buttonEventFilterReset_Click(object sender, EventArgs e)
        {
            checkBoxEventNameSearch.Checked = false;
            textBoxEventNameSearch.Clear();
            checkBoxEventPlaceSearch.Checked = false;
            textBoxEventPlaceSearch.Clear();
            checkBoxEventTopicSearch.Checked = false;
            textBoxEventTopicSearch.Clear();
            checkBoxEventDateSearch.Checked = false;
            dateTimePickerEventDateSearchFrom.Value = DateTime.Now;
            dateTimePickerEventDateSearchUntil.Value = DateTime.Now;
            checkBoxEventPriceSearch.Checked = false;
            textBoxEventPriceSearchFrom.Clear();
            textBoxEventPriceSearchUntil.Clear();
            /*checkBoxEventTrafficSearch.Checked = false;
            comboBoxEventTrafficSearch.SelectedIndex = -1;
            textBoxEventTrafficSearchFrom.Clear();
            textBoxEventTrafficSearchUntil.Clear();
            checkBoxEventExpensesSearch.Checked = false;
            textBoxEventExpensesSearchFrom.Clear();
            textBoxEventExpensesSearchUntil.Clear();*/
            dataGridViewEvents.DataSource = controller.EventsTable();
            ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
        }

        //Функция нажатия на кнопку "Фильтр" для скрытия и раскрытия окна фильтра
        private void buttonEventFilter_Click(object sender, EventArgs e)
        {
            if (groupBoxEventSearch.Visible == true) groupBoxEventSearch.Visible = false;
            else groupBoxEventSearch.Visible = true;
        }

        //Функция нажатия на кнопку "Подтвердить" для фильтрации Мероприятий 
        private void buttonEventFilterConfirm_Click(object sender, EventArgs e)
        {
            string wHERE = "";
            eventWhere.Clear();
            eventWhere.Add("WHERE");
            if (checkBoxEventNameSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventNameSearch.Text) ) eventWhere.Add($"Мер.Название LIKE '{textBoxEventNameSearch.Text}%'");
            if (checkBoxEventPlaceSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventPlaceSearch.Text)) eventWhere.Add($"Мест.Название LIKE '{textBoxEventPlaceSearch.Text}%'");
            if (checkBoxEventTopicSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventTopicSearch.Text)) eventWhere.Add($"Мер.Тематика LIKE '{textBoxEventTopicSearch.Text}%'");
            if (checkBoxEventDateSearch.Checked == true) eventWhere.Add($"Мер.Дата BETWEEN '{dateTimePickerEventDateSearchFrom.Value.ToString("yyyy-MM-dd")}' AND '{dateTimePickerEventDateSearchUntil.Value.ToString("yyyy-MM-dd")}'");
            if (checkBoxEventPriceSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventPriceSearchFrom.Text) && !String.IsNullOrWhiteSpace(textBoxEventPriceSearchUntil.Text))
                eventWhere.Add($"[Цена билета] BETWEEN {textBoxEventPriceSearchFrom.Text} AND {textBoxEventPriceSearchUntil.Text}");

            /*if (eventWhere.Count <= 2)
            {
                eventWhere[0] = eventWhere[0] + " " + eventWhere[1];
                wHERE = eventWhere[0];
                dataGridViewEvents.DataSource = controller.EventsTable(wHERE);
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
            }*/
            if (eventWhere.Count > 1)
            {
                eventWhere[0] = eventWhere[0] + " " + eventWhere[1];
                wHERE = String.Join(" AND ", eventWhere);
                dataGridViewEvents.DataSource = controller.EventsTable(wHERE);
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
            }
            else
            {
                dataGridViewEvents.DataSource = controller.EventsTable();
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
            }
            /*if (eventWhere.Count == 2) wHERE = String.Join(" ", eventWhere);
            else if (eventWhere.Count >= 3) wHERE = String.Join(" AND ", eventWhere);*/
        }

        private void textBoxEventPriceSearchFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxEventPriceSearchFrom.Text.Length > 5) e.KeyChar = (char)0;
        }

        private void textBoxEventPriceSearchUntil_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxEventPriceSearchUntil.Text.Length > 5) e.KeyChar = (char)0;
        }
    }
}
