using Istomin_KP_Sobitiyka_RKIS_24ISP.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormCollectives : Form
    {
        Query controller;

        int showMode = 0;

        public FormCollectives()
        {
            InitializeComponent();
            //Метод для добавления к каждой операции привязки данных метода сокрытия служебного столбца с первичным ключом
            dataGridViewCollectives.DataBindingComplete += (s, ev) => HideServiceColumnsCollectives();
            dataGridViewCompositionOfCollectives.DataBindingComplete += (s, ev) => HideServiceColumnsCollectives();


            controller = new Query(AppSetting.ConnStr);

            //Заполнение таблиц-справочников записями
            dataGridViewCollectives.DataSource = controller.CollectivesTable();
            ShowCountOfCollectivesTableEntry(labelCollectivesCount, dataGridViewCollectives);
            showMode = 0;
            dataGridViewCompositionOfCollectives.DataSource = controller.CompositionOfCollectivesTable();
            ShowCountOfCollectivesTableEntry(labelCompositionOfCollectivesCount, dataGridViewCompositionOfCollectives);
            labelCollectivesCompositionShowMode.Text = "Все участники:";
        }

        //Метод для сокрытия от пользователя служебного столбца с первичным ключом
        private void HideServiceColumnsCollectives()
        {
            if (dataGridViewCollectives.Columns.Contains("Код_коллектива"))
                dataGridViewCollectives.Columns["Код_коллектива"].Visible = false;

            if (dataGridViewCompositionOfCollectives.Columns.Contains("Код_участника"))
                dataGridViewCompositionOfCollectives.Columns["Код_участника"].Visible = false;

            if (dataGridViewCompositionOfCollectives.Columns.Contains("Код_коллектива"))
                dataGridViewCompositionOfCollectives.Columns["Код_коллектива"].Visible = false;

            /*if (dataGridViewPreferentialCategories.Columns.Contains("Код_категории"))
                dataGridViewPreferentialCategories.Columns["Код_категории"].Visible = false;*/
        }

        //Функция нажатия на кнопку "Выход"
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из окна 'Коллективы'", "Выход", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            else Close();
        }

        //Метод вывода количества записей
        private void ShowCountOfCollectivesTableEntry(Label label, DataGridView dataGridView)
        {
            label.Text = $"{(dataGridView.RowCount).ToString()}";
        }

        //Функция заполнения состава коллектива при нажатии на любую часть ячейки Коллективов
        private void dataGridViewCollectives_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            showMode = 1;
            DataGridViewRow row = dataGridViewCollectives.CurrentRow;
            // Читаем значения из ячеек выбранной строки
            int id = Convert.ToInt32(row.Cells["Код_коллектива"].Value);
            //Заполнение вспомогательных таблиц данными, на основе выбранной строки в главной таблице
            string where = $" WHERE СК.Код_коллектива = {id}";
            dataGridViewCompositionOfCollectives.DataSource = controller.CompositionOfCollectivesTable(where);
            ShowCountOfCollectivesTableEntry(labelCompositionOfCollectivesCount, dataGridViewCompositionOfCollectives);

            string name = row.Cells["Название коллектива"].Value.ToString();
            labelCollectivesSelectedName.Text = name;

            labelCollectivesCompositionShowMode.Text = "Участники коллектива:";
        }

        //Функция при нажатии на кнопку "Показать всех участвников" для отображения всех участников когда-либо записаных в БД
        private void buttonCollectivesShowAllComposition_Click(object sender, EventArgs e)
        {
            showMode = 0;
            dataGridViewCompositionOfCollectives.DataSource = controller.CompositionOfCollectivesTable();
            ShowCountOfCollectivesTableEntry(labelCompositionOfCollectivesCount, dataGridViewCompositionOfCollectives);

            labelCollectivesCompositionShowMode.Text = "Все участники:";
        }

        //Функция изменения значения label на имя и дату рож. выбранного участника
        private void UpdateSelectedComposition()
        {
            DataGridViewRow row = dataGridViewCompositionOfCollectives.CurrentRow;
            string name = row.Cells["ФИО"].Value.ToString();
            string DateStr = row.Cells["Дата рождения"].Value.ToString();
            labelCompositionOfCollectivesSelectedNameAndDate.Text = name + "  " + DateStr;
        }

        //Функция при нажатии на строку таблицы с участниками коллективов
        private void dataGridViewCompositionOfCollectives_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSelectedComposition();
        }

        //Метод открытия формы правки Ответсвтвенных
        private void OpenFormCorrectionCollectives(int mode)
        {
            FormCorrectionCollectives formCorrectionCollectives;
            if (mode == 1) formCorrectionCollectives = new FormCorrectionCollectives();
            else
            {
                DataGridViewRow row = dataGridViewCollectives.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Код_коллектива"].Value);
                string name = row.Cells["Название коллектива"].Value.ToString();
                formCorrectionCollectives = new FormCorrectionCollectives(id, name);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionCollectives.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewCollectives.DataSource = controller.CollectivesTable();
                ShowCountOfCollectivesTableEntry(labelCompositionOfCollectivesCount, dataGridViewCompositionOfCollectives);
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Функция нажатия на кнопку "Добавть Коллектив"
        private void добавтьКоллективToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int mode = 1;
            OpenFormCorrectionCollectives(mode);
        }

        //Функция нажатия на кнопку "Редактировать Коллектив"
        private void редактироватьКоллективToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int mode = 2;
            OpenFormCorrectionCollectives(mode);
        }

        //Функция нажатия на кнопку "Удалить Коллектив"
        private void удалитьКоллективToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int mode = 3;
            DataGridViewRow row = dataGridViewCollectives.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_коллектива"].Value);
            if (controller.IsCollectivesUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить коллектив " +
                                                            $"{dataGridViewCollectives.CurrentRow.Cells["Название коллектива"].Value}?", 
                                                            "Подтверждение", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes && controller.ModifyCollectives(mode, id, "") > 0)
                //{
                    controller.ModifyCollectives(mode, id, "");
                    MessageBox.Show($"Запись была удалена!");
                    dataGridViewCollectives.DataSource = controller.CollectivesTable();
                    //ShowCountOfSpravTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
                //}
            }
            else
            {
                MessageBox.Show("Нельзя удалить коллектив, так как он учавствует в организации мероприятия!", "Ошибка!");
                return;
            }
        }

        //Функция нажатия на кнопку "Удалить Коллектив и его участников"
        private void удалитьКоллективИЕгоУчастниковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int mode = 4;
            DataGridViewRow row = dataGridViewCollectives.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_коллектива"].Value);
            if (controller.IsCollectivesUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить коллектив " +
                                                            $"{dataGridViewCollectives.CurrentRow.Cells["Название коллектива"].Value} и его участников?",
                                                            "Подтверждение", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes && controller.ModifyCollectives(mode, id, "") > 0)
                //{
                controller.ModifyCollectives(mode, id, "");
                    MessageBox.Show($"Записи были удалены!");
                    dataGridViewCollectives.DataSource = controller.CollectivesTable();
                    //ShowCountOfSpravTableEntry(toolStripStatusLabelResponsible, dataGridViewResponsible);
                //}
            }
            else
            {
                MessageBox.Show("Нельзя удалить коллектив, так как он учавствует в организации мероприятия!", "Ошибка!");
                return;
            }
        }

        //Метод открытия формы правки Участников коллектива
        private void OpenFormCorrectionComposition(int mode)
        {
            FormCorrectionComposition formCorrectionComposition;
            if (mode == 1) formCorrectionComposition = new FormCorrectionComposition();
            else
            {
                DataGridViewRow row = dataGridViewCompositionOfCollectives.CurrentRow;
                // Читаем значения из ячеек выбранной строки
                int id = Convert.ToInt32(row.Cells["Код_участника"].Value);
                string name = row.Cells["ФИО"].Value.ToString();
                string post = row.Cells["Должность"].Value.ToString();
                string DateStr = row.Cells["Дата рождения"].Value.ToString();
                DateTime Date = DateTime.Parse(DateStr);
                string genderText = row.Cells["Пол"].Value.ToString();
                int gender;
                if (genderText == "Мужской") gender = 0;
                else gender = 1;
                //label2.Text = row.Cells["Код_коллектива"].Value.ToString();
                int collectiveID;
                if (String.IsNullOrWhiteSpace(row.Cells["Код_коллектива"].Value.ToString())) collectiveID = -10;
                else collectiveID = Convert.ToInt32(row.Cells["Код_коллектива"].Value);
                formCorrectionComposition = new FormCorrectionComposition(id, name, post, Date, gender, collectiveID);
            }
            // Блокируем доступ к исходной форме
            this.Enabled = false;
            DialogResult result = formCorrectionComposition.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dataGridViewCompositionOfCollectives.DataSource = controller.CompositionOfCollectivesTable();
                ShowCountOfCollectivesTableEntry(labelCompositionOfCollectivesCount, dataGridViewCompositionOfCollectives);
            }
            // После закрытия form2 возвращаемся к исходной форме
            this.Enabled = true;
        }

        //Функция нажатия на кнопку "Добавить"
        private void buttonCompositionOfCollectivesAdd_Click(object sender, EventArgs e)
        {
            int mode = 1;
            OpenFormCorrectionComposition(mode);
        }

        //Функция нажатия на кнопку "Редактировать"
        private void buttonCompositionOfCollectivesEdit_Click(object sender, EventArgs e)
        {
            int mode = 2;
            OpenFormCorrectionComposition(mode);
        }

        //Функция нажатия на кнопку "Удалить"
        private void buttonCompositionOfCollectivesDelete_Click(object sender, EventArgs e)
        {
            int mode = 3;
            DataGridViewRow row = dataGridViewCompositionOfCollectives.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Код_участника"].Value);
            if (controller.IsCompositionUsed(id) == true)
            {
                DialogResult dialogResult = MessageBox.Show($"Вы уверены, что хотите удалить участника " +
                                                            $"{dataGridViewCompositionOfCollectives.CurrentRow.Cells["ФИО"].Value}, " +
                                                            $"{dataGridViewCompositionOfCollectives.CurrentRow.Cells["Должность"].Value}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes && controller.ModifyComposition(mode, id, "", "", DateTime.Today, "", 0, false) > 0)
                {
                    MessageBox.Show($"Запись была удалена!");
                    dataGridViewCompositionOfCollectives.DataSource = controller.CompositionOfCollectivesTable();
                    ShowCountOfCollectivesTableEntry(labelCompositionOfCollectivesCount, dataGridViewCompositionOfCollectives);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить ответстванного, так как он учавствует в организации мероприятия!", "Ошибка!");
                return;
            }
        }
    }
}
