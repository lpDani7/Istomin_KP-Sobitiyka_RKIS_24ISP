using ClosedXML.Excel;
using Istomin_KP_Sobitiyka_RKIS_24ISP.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormCorrectionEvent : Form
    {

        Query controller;
        int mode = 0;
        int id = 0;
        string name;
        DateTime Date;
        int placeID;
        int price;
        string topic;
        int traffic;
        bool readyToExport = false;

        public FormCorrectionEvent()
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            this.Text = "Добавление Мероприятия";
            mode = 1;
            this.Width = 653;
            this.Height = 340;

            //Очистка полей
            label7.Text = "";
            labelCorrectionEventID.Text = "";
            dateTimePickerCorrectionEventDate.Value = DateTime.Now;
            textBoxCorrectionEventName.Clear();
            textBoxCorrectionEventTopic.Clear();
            textBoxCorrectionEventPrice.Clear();
            textBoxCorrectionEventTraffic.Clear();

            //Заполнение списка мест проведения
            comboBoxCorrectionEventPlaces.Items.Clear();
            comboBoxCorrectionEventPlaces.DataSource = controller.PlacesIDAndNameTable();
            comboBoxCorrectionEventPlaces.ValueMember = "Код_объекта";
            comboBoxCorrectionEventPlaces.DisplayMember = "Название";
            comboBoxCorrectionEventPlaces.SelectedIndex = -1;
        }

        public FormCorrectionEvent(int id, string name, DateTime Date, int placeID, int price, string topic, int traffic)
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            this.Text = "Редактирование Мероприятия";
            this.id = id;
            mode = 2;
            buttonCorrectionEventAddWithOutConnection.Visible = false;
            buttonCorrectionEventAddAndConnection.Visible = false;
            buttonCorrectionEventCancelUp.Visible = false;

            //Заполнение полей информацией о выбранном мероприятии
            labelCorrectionEventID.Text = id.ToString();
            this.Date = Date;
            dateTimePickerCorrectionEventDate.Value = Date;
            this.name = name;
            textBoxCorrectionEventName.Text = name;
            this.topic = topic;
            textBoxCorrectionEventTopic.Text = topic;
            this.price = price;
            textBoxCorrectionEventPrice.Text = price.ToString();
            this.traffic = traffic;
            textBoxCorrectionEventTraffic.Text = traffic.ToString();
            this.placeID = placeID;

            //Заполнение списка мест проведения и выбор того, что стоит в редактируемом мероприятии
            comboBoxCorrectionEventPlaces.Items.Clear();
            comboBoxCorrectionEventPlaces.DataSource = controller.PlacesIDAndNameTable();
            comboBoxCorrectionEventPlaces.ValueMember = "Код_объекта";
            comboBoxCorrectionEventPlaces.DisplayMember = "Название";
            comboBoxCorrectionEventPlaces.SelectedValue = placeID;

            //Заполнение таблиц и comboBox
            FillCorrectionComboBoxAndCorrectionTableByEventID(id);
        }

        //Функция создания мероприятия со связами или без
        private void AddEventWithOrWithOutConnection(int buttonAddMode)
        {
            //Защита от ввода пустых значений
            if (String.IsNullOrWhiteSpace(textBoxCorrectionEventName.Text)) { MessageBox.Show("Ошибка ввода Названия!"); return; }
            if (String.IsNullOrWhiteSpace(textBoxCorrectionEventTopic.Text)) { MessageBox.Show("Ошибка ввода Тематики!"); return; }
            if (String.IsNullOrWhiteSpace(textBoxCorrectionEventPrice.Text)) { MessageBox.Show("Ошибка ввода Цены билета!"); return; }
            if (comboBoxCorrectionEventPlaces.SelectedIndex == -1) { MessageBox.Show("Выберите место проведения!"); return; }

            //Получение данных для созданиия записи
            string Date = dateTimePickerCorrectionEventDate.Value.ToString("yyyy-MM-dd");
            string name = textBoxCorrectionEventName.Text.Trim();
            string topic = textBoxCorrectionEventTopic.Text.Trim();
            int placeID = Convert.ToInt32(comboBoxCorrectionEventPlaces.SelectedValue);
            int price = Convert.ToInt32(textBoxCorrectionEventPrice.Text.Trim());
            int traffic = 0;
            if (!String.IsNullOrWhiteSpace(textBoxCorrectionEventTraffic.Text))
            {
                traffic = Convert.ToInt32(textBoxCorrectionEventTraffic.Text.Trim());
            }
            else textBoxCorrectionEventTraffic.Text = traffic.ToString();

            //Проверка на совпадения в БД
            if (controller.IsEventHasCoincidence(name, Date, topic, placeID, price) == false && (mode == 1 || mode == 2))
            {
                if (mode == 1) { MessageBox.Show("Запись с такими данными уже существует 1!"); return; }
                else if ((name != this.name || Convert.ToDateTime(Date) != this.Date || topic != this.topic || placeID != this.placeID || price != this.price /*|| traffic != this.traffic*/) && mode == 2) { MessageBox.Show("Запись с такими данными уже существует 2!"); return; }
            }
            if (controller.ModifyEvent(mode, id, name, Date, topic, placeID, price, traffic) > 0)
            {
                if (mode == 1) MessageBox.Show($"{name}  {dateTimePickerCorrectionEventDate.Value.ToString("dd.MM.yyyy")} добавлено!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                if (buttonAddMode == 2) id = controller.IDEventByData(name, Date, topic, placeID, price, traffic);
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                readyToExport = true;
            }

            /*else if (controller.IsEventHasCoincidence(name, Date, topic, placeID, price) == false && mode == 2 *//*&& name != this.name && Convert.ToDateTime(Date) != this.Date && topic != this.topic && placeID != this.placeID && price != this.price && traffic != this.traffic*//*)
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }*/
        }

        //Функция для заполнения всех таблиц и комбо боксов в форме редактирования
        private void FillCorrectionComboBoxAndCorrectionTableByEventID(int id)
        {
            //Сокрытие служебных столбцов при загрузке таблиц
            dataGridViewCorrectionEventResponsibleAtEvent.DataBindingComplete += (s, ev) => HideServiceColumnsResponsibleAtEvent();
            dataGridViewCorrectionEventResponsibleAll.DataBindingComplete += (s, ev) => HideServiceColumnsResponsibleAll();
            dataGridViewCorrectionEventCollectives.DataBindingComplete += (s, ev) => HideServiceColumnsPerformers();
            dataGridViewCorrectionEventTypesOfExpenses.DataBindingComplete += (s, ev) => HideServiceColumnsExpenses();
            dataGridViewCorrectionEventPreferentialCategories.DataBindingComplete += (s, ev) => HideServiceColumnsPreferentialCategories();

            //Заполнение таблиц
            string where = "";
            where = $" WHERE ОМ.Код_мероприятия = {id}";
            dataGridViewCorrectionEventResponsibleAtEvent.DataSource = controller.ResponsibleForEventTable(where);
            where = $" WHERE Таб_номер NOT IN (SELECT Таб_номер FROM Ответственные_за_мероприятие WHERE Код_мероприятия = {id})";
            dataGridViewCorrectionEventResponsibleAll.DataSource = controller.ResponsibleTable(where);
            where = $" WHERE И.Код_мероприятия = {id}";
            dataGridViewCorrectionEventCollectives.DataSource = controller.EventPerformersTable(where);
            where = $" WHERE С.Код_мероприятия = {id}";
            dataGridViewCorrectionEventTypesOfExpenses.DataSource = controller.ExpensesForEventTable(where);
            where = $" WHERE ЛП.Код_мероприятия = {id}";
            dataGridViewCorrectionEventPreferentialCategories.DataSource = controller.PreferentialCategoriesAtEventTable(where);

            //Заполнение comboBox
            comboBoxCorrectionEventPreferentialCategories.Items.Clear();
            comboBoxCorrectionEventPreferentialCategories.DataSource = controller.PreferentialCategoriesIDAndNameAndPercentTable();
            comboBoxCorrectionEventPreferentialCategories.ValueMember = "Код_категории";
            comboBoxCorrectionEventPreferentialCategories.DisplayMember = "Название_Послабление";
            comboBoxCorrectionEventPreferentialCategories.SelectedIndex = -1;

            comboBoxCorrectionEventCollectives.Items.Clear();
            comboBoxCorrectionEventCollectives.DataSource = controller.CollectivesIDAndNameTable(id);
            comboBoxCorrectionEventCollectives.ValueMember = "Код_коллектива";
            comboBoxCorrectionEventCollectives.DisplayMember = "Название";
            comboBoxCorrectionEventCollectives.SelectedIndex = -1;

            comboBoxCorrectionEventTypesOfExpenses.Items.Clear();
            comboBoxCorrectionEventTypesOfExpenses.DataSource = controller.TypesOfExpensesIDAndNameTable();
            comboBoxCorrectionEventTypesOfExpenses.ValueMember = "Код_записи";
            comboBoxCorrectionEventTypesOfExpenses.DisplayMember = "Тип_расхода";
            comboBoxCorrectionEventTypesOfExpenses.SelectedIndex = -1;

        }

        //Фукнция нажатия на кнопку "Создать мероприятие без связей"
        private void buttonCorrectionEventAddWithOutConnection_Click(object sender, EventArgs e)
        {
            int buttonAddMode = 1;
            mode = 1;
            AddEventWithOrWithOutConnection(buttonAddMode);
        }

        //Фукнция нажатия на кнопку "Создать мероприятие и добавить связи"
        private void buttonCorrectionEventAddAndConnection_Click(object sender, EventArgs e)
        {
            int buttonAddMode = 2;
            mode = 1;
            AddEventWithOrWithOutConnection(buttonAddMode);
            if (id > 0)
            {
                this.Width = 1465;
                this.Height = 801;
                buttonCorrectionEventAddWithOutConnection.Visible = false;
                buttonCorrectionEventAddAndConnection.Visible = false;
                buttonCorrectionEventCancelUp.Visible = false;
                label7.Visible = true;
                label7.Text = "Номер:";
                labelCorrectionEventID.Text = id.ToString();
                FillCorrectionComboBoxAndCorrectionTableByEventID(id);
                mode = 2;
                this.Date = dateTimePickerCorrectionEventDate.Value;
                this.name = textBoxCorrectionEventName.Text;
                this.topic = textBoxCorrectionEventTopic.Text;
                this.price = Convert.ToInt32(textBoxCorrectionEventPrice.Text);
                this.traffic = Convert.ToInt32(textBoxCorrectionEventTraffic.Text);
                this.placeID = Convert.ToInt32(comboBoxCorrectionEventPlaces.SelectedValue);
            }
        }

        //Фукнция нажатия на кнопку "Отмена" снизу
        private void buttonCorrectionEventCancelDown_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Фукнция нажатия на кнопку "Отмена" сверху
        private void buttonCorrectionEventCancelUp_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Метод для сокрытия от пользователя служебных столбцов Ответственных за мероприятие
        private void HideServiceColumnsResponsibleAtEvent()
        {
            if (dataGridViewCorrectionEventResponsibleAtEvent.Columns.Contains("Код_записи"))
                dataGridViewCorrectionEventResponsibleAtEvent.Columns["Код_записи"].Visible = false;
            if (dataGridViewCorrectionEventResponsibleAtEvent.Columns.Contains("Таб_номер"))
                dataGridViewCorrectionEventResponsibleAtEvent.Columns["Таб_номер"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов всех Ответственных
        private void HideServiceColumnsResponsibleAll()
        {
            if (dataGridViewCorrectionEventResponsibleAll.Columns.Contains("Таб_номер"))
                dataGridViewCorrectionEventResponsibleAll.Columns["Таб_номер"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов Исполнителей
        private void HideServiceColumnsPerformers()
        {
            if (dataGridViewCorrectionEventCollectives.Columns.Contains("Код_записи"))
                dataGridViewCorrectionEventCollectives.Columns["Код_записи"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов Расходов на мероприятие
        private void HideServiceColumnsExpenses()
        {
            if (dataGridViewCorrectionEventTypesOfExpenses.Columns.Contains("Номер_записи"))
                dataGridViewCorrectionEventTypesOfExpenses.Columns["Номер_записи"].Visible = false;
        }

        //Метод для сокрытия от пользователя служебных столбцов Льготной посещаемости
        private void HideServiceColumnsPreferentialCategories()
        {
            if (dataGridViewCorrectionEventPreferentialCategories.Columns.Contains("Номер_записи"))
                dataGridViewCorrectionEventPreferentialCategories.Columns["Номер_записи"].Visible = false;
        }

        //Функция поиска по первым буквам фамилии при изменении текста
        private void textBoxCorrectionEventResponsibleAllSearch_TextChanged(object sender, EventArgs e)
        {
            string where = $" WHERE Таб_номер NOT IN (SELECT Таб_номер FROM Ответственные_за_мероприятие WHERE Код_мероприятия = {id})" +
                           $" and ФИО LIKE '{textBoxCorrectionEventResponsibleAllSearch.Text.Trim()}%'";
            dataGridViewCorrectionEventResponsibleAll.DataSource = controller.ResponsibleTable(where);
        }

        //Функция нажатия кнопки "Добавить" для Льготной посещаемости
        private void buttonCorrectionEventPreferentialCategoriesAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxCorrectionEventPreferentialCategories.SelectedIndex == -1) { MessageBox.Show("Выберите льготную категорию!"); return; }
            if (String.IsNullOrWhiteSpace(textBoxCorrectionEventPreferentialTraffic.Text)) { MessageBox.Show("Ошибка ввода Количества!", "Ошибка"); return; }
            int idEvent = this.id;
            int category = Convert.ToInt32(comboBoxCorrectionEventPreferentialCategories.SelectedValue);
            int count = Convert.ToInt32(textBoxCorrectionEventPreferentialTraffic.Text);
            int mode = 1;
            if (controller.IsPreferentialAttendanceHasCoincidence(idEvent, category) == false)
            {
                mode = 2;
                controller.ModifyPreferentialAttendance(mode, idEvent, 0, category, count);
                MessageBox.Show("Запись обновлена!");
            }
            else if (controller.ModifyPreferentialAttendance(mode, idEvent, 0, category, count) > 0)
            {
                MessageBox.Show("Запись добавлена!");
            }
            else MessageBox.Show("Произошла ошибка!");

            string where = $" WHERE ЛП.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventPreferentialCategories.DataSource = controller.PreferentialCategoriesAtEventTable(where);
            comboBoxCorrectionEventPreferentialCategories.SelectedIndex = -1;
            textBoxCorrectionEventPreferentialTraffic.Clear();
        }

        //Функция нажатия кнопки "Удалить" для Льготной посещаемости
        private void buttonCorrectionEventPreferentialCategoriesDelete_Click(object sender, EventArgs e)
        {
            int mode = 3;
            DataGridViewRow row = dataGridViewCorrectionEventPreferentialCategories.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Номер_записи"].Value);
            DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить категорию " +
                                                            $"{dataGridViewCorrectionEventPreferentialCategories.CurrentRow.Cells["Название"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                controller.ModifyPreferentialAttendance(mode, 0, id, 0, 0);
                MessageBox.Show($"Запись была удалена!");
            }
            string where = $" WHERE ЛП.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventPreferentialCategories.DataSource = controller.PreferentialCategoriesAtEventTable(where);
        }

        //Функция нажатия кнопки "Добавить" для Исполнителей
        private void buttonCorrectionEventCollectivesAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxCorrectionEventCollectives.SelectedIndex == -1) { MessageBox.Show("Выберите коллектив!"); return; }
            int idEvent = this.id;
            int collectives = Convert.ToInt32(comboBoxCorrectionEventCollectives.SelectedValue);
            int mode = 1;
            if (controller.ModifyPerformers(mode, idEvent, 0, collectives) > 0)
            {
                MessageBox.Show("Запись добавлена!");
            }
            else MessageBox.Show("Произошла ошибка!");

            string where = $" WHERE И.Код_мероприятия = {id}";
            dataGridViewCorrectionEventCollectives.DataSource = controller.EventPerformersTable(where);
            comboBoxCorrectionEventCollectives.DataSource = controller.CollectivesIDAndNameTable(this.id);
            comboBoxCorrectionEventCollectives.ValueMember = "Код_коллектива";
            comboBoxCorrectionEventCollectives.DisplayMember = "Название";
            comboBoxCorrectionEventCollectives.SelectedIndex = -1;
        }

        //Функция нажатия кнопки "Удалить" для Исполнителей
        private void buttonCorrectionEventCollectivesDelete_Click(object sender, EventArgs e)
        {
            int mode = 2;
            DataGridViewRow row = dataGridViewCorrectionEventCollectives.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_записи"].Value);
            DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить коллектив " +
                                                            $"{dataGridViewCorrectionEventCollectives.CurrentRow.Cells["Название"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                controller.ModifyPerformers(mode, 0, id, 0);
                MessageBox.Show($"Запись была удалена!");
            }
            string where = $" WHERE И.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventCollectives.DataSource = controller.EventPerformersTable(where);
            comboBoxCorrectionEventCollectives.DataSource = controller.CollectivesIDAndNameTable(this.id);
            comboBoxCorrectionEventCollectives.ValueMember = "Код_коллектива";
            comboBoxCorrectionEventCollectives.DisplayMember = "Название";
            comboBoxCorrectionEventCollectives.SelectedIndex = -1;
        }

        //Функция нажатия кнопки "Добавить" для Сметы
        private void buttonCorrectionEventTypesOfExpensesAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxCorrectionEventTypesOfExpenses.SelectedIndex == -1) { MessageBox.Show("Выберите тип расхода!"); return; }
            if (String.IsNullOrWhiteSpace(textBoxCorrectionEventExpenses.Text)) { MessageBox.Show("Ошибка ввода Затрат!", "Ошибка"); return; }
            if (String.IsNullOrWhiteSpace(textBoxCorrectionEventDescription.Text)) { MessageBox.Show("Ошибка ввода Описания!", "Ошибка"); return; }
            int idEvent = this.id;
            int type = Convert.ToInt32(comboBoxCorrectionEventTypesOfExpenses.SelectedValue);
            int expenses = Convert.ToInt32(textBoxCorrectionEventExpenses.Text);
            string description = textBoxCorrectionEventDescription.Text.Trim();
            int mode = 1;
            if (controller.IsExpensesForEventHasCoincidence(idEvent, type, description) == false)
            {
                mode = 2;
                controller.ModifyExpensesForEvent(mode, idEvent, 0, type, expenses, description);
                MessageBox.Show("Запись обновлена!");
            }
            else if (controller.ModifyExpensesForEvent(mode, idEvent, 0, type, expenses, description) > 0)
            {
                MessageBox.Show("Запись добавлена!");
            }
            else MessageBox.Show("Произошла ошибка!");

            string where = $" WHERE С.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventTypesOfExpenses.DataSource = controller.ExpensesForEventTable(where);
            comboBoxCorrectionEventTypesOfExpenses.SelectedIndex = -1;
            textBoxCorrectionEventExpenses.Clear();
            textBoxCorrectionEventDescription.Clear();
        }

        //Функция нажатия кнопки "Удалить" для Сметы
        private void buttonCorrectionEventTypesOfExpensesDelete_Click(object sender, EventArgs e)
        {
            int mode = 3;
            DataGridViewRow row = dataGridViewCorrectionEventTypesOfExpenses.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Номер_записи"].Value);
            DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить ответственного " +
                                                            $"{dataGridViewCorrectionEventTypesOfExpenses.CurrentRow.Cells["Тип расхода"].Value}, " +
                                                            $"{dataGridViewCorrectionEventTypesOfExpenses.CurrentRow.Cells["Описание"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                controller.ModifyExpensesForEvent(mode, 0, id, 0, 0, "");
                MessageBox.Show($"Запись была удалена!");
            }
            string where = $" WHERE С.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventTypesOfExpenses.DataSource = controller.ExpensesForEventTable(where);
        }

        //Функция нажатия кнопки "Добавить" для Ответственных за мероприятие
        private void buttonCorrectionEventResponsibleAllAdd_Click(object sender, EventArgs e)
        {
            if (dataGridViewCorrectionEventResponsibleAll.CurrentRow.Index == -1) { MessageBox.Show("Выберите ответственного!"); return; }
            int idEvent = this.id;
            DataGridViewRow row = dataGridViewCorrectionEventResponsibleAll.CurrentRow;
            int responsible = Convert.ToInt32(row.Cells["Таб_номер"].Value);
            int mode = 1;
            if (controller.ModifyResponsibleForEvent(mode, idEvent, 0, responsible) > 0)
            {
                MessageBox.Show("Запись добавлена!");
            }
            else MessageBox.Show("Произошла ошибка!");

            string where = $" WHERE ОМ.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventResponsibleAtEvent.DataSource = controller.ResponsibleForEventTable(where);
            where = $" WHERE Таб_номер NOT IN (SELECT Таб_номер FROM Ответственные_за_мероприятие WHERE Код_мероприятия = {this.id})";
            dataGridViewCorrectionEventResponsibleAll.DataSource = controller.ResponsibleTable(where);
        }

        //Функция нажатия кнопки "Удалить" для Ответственных за мероприятие
        private void buttonCorrectionEventResponsibleAtEventDelete_Click(object sender, EventArgs e)
        {
            int mode = 2;
            DataGridViewRow row = dataGridViewCorrectionEventResponsibleAtEvent.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_записи"].Value);
            DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить ответственного " +
                                                            $"{dataGridViewCorrectionEventResponsibleAtEvent.CurrentRow.Cells["ФИО"].Value}, " +
                                                            $"{dataGridViewCorrectionEventResponsibleAtEvent.CurrentRow.Cells["Должность"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                controller.ModifyResponsibleForEvent(mode, 0, id, 0);
                MessageBox.Show($"Запись была удалена!");
            }
            string where = $" WHERE ОМ.Код_мероприятия = {this.id}";
            dataGridViewCorrectionEventResponsibleAtEvent.DataSource = controller.ResponsibleForEventTable(where);
            where = $" WHERE Таб_номер NOT IN (SELECT Таб_номер FROM Ответственные_за_мероприятие WHERE Код_мероприятия = {this.id})";
            dataGridViewCorrectionEventResponsibleAll.DataSource = controller.ResponsibleTable(where);
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 20 символов
        private void textBoxCorrectionEventPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxCorrectionEventPrice.Text.Length > 20) e.KeyChar = (char)0;
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 20 символов
        private void textBoxCorrectionEventTraffic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxCorrectionEventTraffic.Text.Length > 20) e.KeyChar = (char)0;
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 20 символов
        private void textBoxCorrectionEventPreferentialTraffic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8 || e.KeyChar == '-'))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxCorrectionEventPreferentialTraffic.Text.Length > 20) e.KeyChar = (char)0;
        }

        //Ограничение на ввод любых символов, короме цифр и backspace, а также длины не более 20 символов
        private void textBoxCorrectionEventExpenses_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8 || e.KeyChar == '-'))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxCorrectionEventExpenses.Text.Length > 20) e.KeyChar = (char)0;
        }

        //Функция нажатия на кнопку "Подтвердить"
        private void buttonCorrectionEventConfirm_Click(object sender, EventArgs e)
        {
            mode = 2;
            AddEventWithOrWithOutConnection(1);
        }

        //Функция создания в Excel сводки мероприятия
        private void ExportSummaryDataOfEvent()
        {
            {
                // Диалоговое окно для выбора пути сохранения
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "ExportedDataEventCard.xlsx";
                //Можно в имя файла поместить текущую дату

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    { 
                        // Создаём новый документ Excel
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            // Добавляем лист и называем его
                            IXLWorksheet worksheet = workbook.Worksheets.Add("Данные");

                            // Структура сводки
                            int l;
                            worksheet.Cell(1, 1).Value = $"Карта мероприятия: {labelCorrectionEventID.Text}";
                            worksheet.Cell(2, 1).Value = $"==============================================================================================================================";
                            worksheet.Cell(3, 1).Value = $"Название: "; worksheet.Cell(3, 2).Value = textBoxCorrectionEventName.Text.Trim();
                            worksheet.Cell(4, 1).Value = $"Дата: "; worksheet.Cell(4, 2).Value = dateTimePickerCorrectionEventDate.Value.ToShortDateString();
                            worksheet.Cell(5, 1).Value = "Тематика: "; worksheet.Cell(5, 2).Value = textBoxCorrectionEventTopic.Text.Trim();
                            worksheet.Cell(6, 1).Value = "Место проведения: "; worksheet.Cell(6, 2).Value = comboBoxCorrectionEventPlaces.Text.Trim();
                            worksheet.Cell(7, 1).Value = "Цена билета: "; worksheet.Cell(7, 2).Value = Convert.ToInt32(textBoxCorrectionEventPrice.Text.Trim());
                            worksheet.Cell(8, 1).Value = "Посещаемость: "; worksheet.Cell(8, 2).Value = Convert.ToInt32(textBoxCorrectionEventTraffic.Text.Trim());
                            worksheet.Cell(9, 1).Value = $"==============================================================================================================================";
                            worksheet.Cell(10, 1).Value = $"Ответственные:   {dataGridViewCorrectionEventResponsibleAtEvent.Rows.Count}";
                            for (int i = 0; i < dataGridViewCorrectionEventResponsibleAtEvent.Columns.Count-2; i++)
                            {
                                worksheet.Cell(11, i + 1).Value = dataGridViewCorrectionEventResponsibleAtEvent.Columns[i].HeaderText;
                            }
                            l = 12;
                            for (int i = 0; i < dataGridViewCorrectionEventResponsibleAtEvent.Rows.Count; i++)
                            {
                                l++;
                                for (int j = 0; j < dataGridViewCorrectionEventResponsibleAtEvent.Columns.Count-2; j++)
                                {
                                    // Проверяем, что ячейка не пустая
                                    if (dataGridViewCorrectionEventResponsibleAtEvent.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cell(i + 12, j + 1).Value = dataGridViewCorrectionEventResponsibleAtEvent.Rows[i].Cells[j].Value.ToString();
                                    }
                                }
                            }
                            worksheet.Cell(l, 1).Value = $"==============================================================================================================================";
                            worksheet.Cell(l+1, 1).Value = $"Коллективы исполнители:   {dataGridViewCorrectionEventCollectives.Rows.Count}";
                            for (int i = 0; i < dataGridViewCorrectionEventCollectives.Columns.Count-1; i++)
                            {
                                worksheet.Cell(l+2, i + 1).Value = dataGridViewCorrectionEventCollectives.Columns[i].HeaderText;
                            }
                            l+=3;
                            for (int i = 0; i < dataGridViewCorrectionEventCollectives.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridViewCorrectionEventCollectives.Columns.Count-1; j++)
                                {
                                    // Проверяем, что ячейка не пустая
                                    if (dataGridViewCorrectionEventCollectives.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cell(i + l, j + 1).Value = dataGridViewCorrectionEventCollectives.Rows[i].Cells[j].Value.ToString();
                                    }
                                }
                            }
                            l += dataGridViewCorrectionEventCollectives.Rows.Count;
                            worksheet.Cell(l, 1).Value = $"==============================================================================================================================";
                            worksheet.Cell(l+1, 1).Value = $"Льготная посещаемость мероприятия: "; worksheet.Cell(l + 1, 2).Value = Convert.ToInt32(controller.PreferentialTrafficSum(Convert.ToInt32(labelCorrectionEventID.Text)));
                            for (int i = 0; i < dataGridViewCorrectionEventPreferentialCategories.Columns.Count - 1; i++)
                            {
                                worksheet.Cell(l + 2, i + 1).Value = dataGridViewCorrectionEventPreferentialCategories.Columns[i].HeaderText;
                            }
                            l += 3;
                            for (int i = 0; i < dataGridViewCorrectionEventPreferentialCategories.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridViewCorrectionEventPreferentialCategories.Columns.Count - 1; j++)
                                {
                                    // Проверяем, что ячейка не пустая
                                    if (dataGridViewCorrectionEventPreferentialCategories.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cell(i + l, j + 1).Value = dataGridViewCorrectionEventPreferentialCategories.Rows[i].Cells[j].Value.ToString();
                                    }
                                }
                            }
                            l += dataGridViewCorrectionEventPreferentialCategories.Rows.Count;
                            worksheet.Cell(l, 1).Value = $"==============================================================================================================================";
                            worksheet.Cell(l + 1, 1).Value = $"Расходы на мероприятие: "; worksheet.Cell(l + 1, 2).Value = Convert.ToInt32(controller.ExpensesSum(Convert.ToInt32(labelCorrectionEventID.Text)));
                            for (int i = 0; i < dataGridViewCorrectionEventTypesOfExpenses.Columns.Count - 1; i++)
                            {
                                worksheet.Cell(l + 2, i + 1).Value = dataGridViewCorrectionEventTypesOfExpenses.Columns[i].HeaderText;
                            }
                            l += 3;
                            for (int i = 0; i < dataGridViewCorrectionEventTypesOfExpenses.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridViewCorrectionEventTypesOfExpenses.Columns.Count - 1; j++)
                                {
                                    // Проверяем, что ячейка не пустая
                                    if (dataGridViewCorrectionEventTypesOfExpenses.Rows[i].Cells[j].Value != null)
                                    {
                                        worksheet.Cell(i + l, j + 1).Value = dataGridViewCorrectionEventTypesOfExpenses.Rows[i].Cells[j].Value.ToString();
                                    }
                                }
                            }
                            l += dataGridViewCorrectionEventTypesOfExpenses.Rows.Count;

                            // Сохраняем файл
                            workbook.SaveAs(saveFileDialog.FileName);
                            //workbook.Close();
                        }
                        MessageBox.Show("Данные успешно экспортированы!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } //try
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } //catch
                }//if
            }
        }

        //Функция нажатия на кнопку "Печать"
        private void buttonCorrectionEventPrint_Click(object sender, EventArgs e)
        {
            mode = 2;
            AddEventWithOrWithOutConnection(1);
            if (readyToExport) ExportSummaryDataOfEvent();
        }
    }
}
