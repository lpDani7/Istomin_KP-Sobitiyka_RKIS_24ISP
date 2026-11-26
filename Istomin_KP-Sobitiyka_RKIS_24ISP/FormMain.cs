using DocumentFormat.OpenXml.Office2010.Excel;
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
using System.Xml.Linq;

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
            dataGridViewResponsibleForEvent.DataBindingComplete += (s, ev) => HideServiceColumnsResponsible();
            dataGridViewEventPerformers.DataBindingComplete += (s, ev) => HideServiceColumnsPerformers();
            dataGridViewExpensesForEvent.DataBindingComplete += (s, ev) => HideServiceColumnsExpenses();
            dataGridViewPreferentialCategoriesAtEvent.DataBindingComplete += (s, ev) => HideServiceColumnsPreferentialCategories();

            controller = new Query(AppSetting.ConnStr);
            dataGridViewEvents.DataSource = controller.EventsTable();
            ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            FillAllEventTable();
        }

        //Метод для сокрытия от пользователя служебных столбцов мероприятия
        private void HideServiceColumnsMain()
        {
            if (dataGridViewEvents.Columns.Contains("Код_мероприятия"))
                dataGridViewEvents.Columns["Код_мероприятия"].Visible = false;

            if (dataGridViewEvents.Columns.Contains("Код_объекта"))
                dataGridViewEvents.Columns["Код_объекта"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов Ответственных за мероприятие
        private void HideServiceColumnsResponsible()
        {
            if (dataGridViewResponsibleForEvent.Columns.Contains("Код_записи"))
                dataGridViewResponsibleForEvent.Columns["Код_записи"].Visible = false;
            if (dataGridViewResponsibleForEvent.Columns.Contains("Таб_номер"))
                dataGridViewResponsibleForEvent.Columns["Таб_номер"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов коллективов исполнителей
        private void HideServiceColumnsPerformers()
        {
            if (dataGridViewEventPerformers.Columns.Contains("Код_записи"))
                dataGridViewEventPerformers.Columns["Код_записи"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов расходов на мероприятие
        private void HideServiceColumnsExpenses()
        {
            if (dataGridViewExpensesForEvent.Columns.Contains("Номер_записи"))
                dataGridViewExpensesForEvent.Columns["Номер_записи"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов льготной посещаемости
        private void HideServiceColumnsPreferentialCategories()
        {
            if (dataGridViewPreferentialCategoriesAtEvent.Columns.Contains("Номер_записи"))
                dataGridViewPreferentialCategoriesAtEvent.Columns["Номер_записи"].Visible = false;
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

        //Функция открытия формы отчётов
        private void отчётыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReport formReport = new FormReport();
            formReport.ShowDialog(this);
        }

        //Функция открытия формы коллективов
        private void коллективыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCollectives formCollectives = new FormCollectives();
            formCollectives.ShowDialog(this);
        }

        //Функция выхода из приложения
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            else Close();
        }

        //Функция отображания окна справки о приложении
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа 'Событийка' предназначена для регистрации и ведения учёта городских культурных мероприятий. " +
                "\nРазработчик: Студент 3 курса ОГТИ Истомин Данил Евгеньевич", "Справка");
        }

        //Метод обновления вспомогательных таблиц
        private void FillAllEventTable()
        {
            if (dataGridViewEvents.CurrentRow != null)
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
            else
            {
                //Заполнение вспомогательных таблиц данными, на основе выбранной строки в главной таблице
                string where = $" WHERE И.Код_мероприятия = -1";
                dataGridViewEventPerformers.DataSource = controller.EventPerformersTable(where);
                where = $" WHERE ОМ.Код_мероприятия = -1";
                dataGridViewResponsibleForEvent.DataSource = controller.ResponsibleForEventTable(where);
                where = $" WHERE С.Код_мероприятия = -1";
                dataGridViewExpensesForEvent.DataSource = controller.ExpensesForEventTable(where);
                where = $" WHERE ЛП.Код_мероприятия = -1";
                dataGridViewPreferentialCategoriesAtEvent.DataSource = controller.PreferentialCategoriesAtEventTable(where);

                //Заполнение label таблиц дополнительной информации
                ShowCountOfMainTableEntry(labelEventResponsibleCount, dataGridViewResponsibleForEvent);
                ShowCountOfMainTableEntry(labelEventPerformersCount, dataGridViewEventPerformers);
                labelEventExpensesSum.Text = "0";
                labelEventPreferentialTrafficSum.Text = "0";
                labelEventGeneralTrafficSum.Text = "0";
                labelEventSelectedNameAndDate.Text = "0";
            }
        }

        //Функция обновления вспомогательных таблиц при нажатии на строку главной таблицы
        private void dataGridViewEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FillAllEventTable();
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
            if (checkBoxEventNameSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventNameSearch.Text)) eventWhere.Add($"Мер.Название LIKE '{textBoxEventNameSearch.Text}%'");
            if (checkBoxEventPlaceSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventPlaceSearch.Text)) eventWhere.Add($"Мест.Название LIKE '%{textBoxEventPlaceSearch.Text}%'");
            if (checkBoxEventTopicSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventTopicSearch.Text)) eventWhere.Add($"Мер.Тематика LIKE '{textBoxEventTopicSearch.Text}%'");
            if (checkBoxEventDateSearch.Checked == true) eventWhere.Add($"Мер.Дата BETWEEN '{dateTimePickerEventDateSearchFrom.Value.ToString("yyyy-MM-dd")}' AND '{dateTimePickerEventDateSearchUntil.Value.ToString("yyyy-MM-dd")}'");
            if (checkBoxEventPriceSearch.Checked == true && !String.IsNullOrWhiteSpace(textBoxEventPriceSearchFrom.Text) && !String.IsNullOrWhiteSpace(textBoxEventPriceSearchUntil.Text))
                eventWhere.Add($"[Цена билета] BETWEEN {textBoxEventPriceSearchFrom.Text} AND {textBoxEventPriceSearchUntil.Text}");

            if (eventWhere.Count > 1)
            {
                eventWhere[0] = eventWhere[0] + " " + eventWhere[1];
                wHERE = String.Join(" AND ", eventWhere);
                dataGridViewEvents.DataSource = controller.EventsTable(wHERE);
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
                FillAllEventTable();
            }
            else
            {
                dataGridViewEvents.DataSource = controller.EventsTable();
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
                FillAllEventTable();
            }
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

        //Метод открытия формы правки Мероприятий
        private void OpenFormCorrectionEvent(int mode)
        {
            FormCorrectionEvent formCorrectionEvent;
            if (mode == 1) formCorrectionEvent = new FormCorrectionEvent();
            else
            {
                DataGridViewRow row = dataGridViewEvents.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Код_мероприятия"].Value);
                string name = row.Cells["Название"].Value.ToString();
                DateTime Date = DateTime.Parse(row.Cells["Дата проведения"].Value.ToString());
                int placeID = Convert.ToInt32(row.Cells["Код_объекта"].Value.ToString());
                int price = Convert.ToInt32(row.Cells["Цена билета"].Value);
                string topic = row.Cells["Тематика"].Value.ToString();
                int traffic = Convert.ToInt32(row.Cells["Посещаемость"].Value);
                formCorrectionEvent = new FormCorrectionEvent(id, name, Date, placeID, price, topic, traffic);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionEvent.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewEvents.DataSource = controller.EventsTable();
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
                FillAllEventTable();
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Функция нажатия кнопки "Добавить"
        private void buttonEventAdd_Click(object sender, EventArgs e)
        {
            int mode = 1;
            OpenFormCorrectionEvent(mode);
        }

        //Функция нажатия кнопки "Редактировать"
        private void buttonEventEdit_Click(object sender, EventArgs e)
        {
            int mode = 2;
            OpenFormCorrectionEvent(mode);
        }

        //Функция нажатия кнопки "Добавить мероприятие"
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int mode = 1;
            OpenFormCorrectionEvent(mode);
        }

        //Функция нажатия кнопки "Редактировать мероприятие"
        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int mode = 2;
            OpenFormCorrectionEvent(mode);
        }

        //Функция удаления мероприятия и связанной с ним информации
        private void DeleteEventAndConnection()
        {
            int mode = 3;
            DataGridViewRow row = dataGridViewEvents.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_мероприятия"].Value);
            DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить мероприятие " +
                                                            $"{dataGridViewEvents.CurrentRow.Cells["Название"].Value}, " +
                                                            $"{dataGridViewEvents.CurrentRow.Cells["Дата проведения"].Value.ToString()} и все его связи? " +
                                                            $"Это действие нельзя будет отменить, все данные об этом мероприятии и " +
                                                            $"связанная с ним информация из других таблиц будут удалены!", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                controller.DeleteEventConnection(id);
                controller.ModifyEvent(mode, id, "", "", "", 0, 0, 0);
                MessageBox.Show($"Мероприятие и его связи были удалены!");
                dataGridViewEvents.DataSource = controller.EventsTable();
                ShowCountOfMainTableEntry(labelEventCount, dataGridViewEvents);
                FillAllEventTable();
            }
        }

        //Функция нажатия кнопки "Удалить"
        private void buttonEventDelete_Click(object sender, EventArgs e)
        {
            DeleteEventAndConnection();
        }

        //Функция нажатия кнопки "Удалить мероприятие"
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteEventAndConnection();
        }
    }
}
