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
using System.Xml.Linq;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormCorrectionCollectives : Form
    {
        int mode = 0;

        private int id;
        Query controller;


        public FormCorrectionCollectives()
        {
            InitializeComponent();

            //Метод для добавления к каждой операции привязки данных метода сокрытия служебного столбца с первичным ключом
            dataGridViewCorrectionCollectivesCompositionFree.DataBindingComplete += (s, ev) => HideServiceColumnsCorrectionCollectives();
            dataGridViewCorrectionCollectivesComposition.DataBindingComplete += (s, ev) => HideServiceColumnsCorrectionCollectives();
            this.Text = "Добавление коллектива";
            mode = 1;
            labelCorrectionCollectivesID.Visible = false;
            this.Height = 111;
            textBoxCorrectionCollectivesName.Clear();

            controller = new Query(AppSetting.ConnStr);
            string where = "WHERE СК.Код_коллектива IS NULL";
            dataGridViewCorrectionCollectivesCompositionFree.DataSource = controller.CompositionOfCollectivesTable(where);
        }

        public FormCorrectionCollectives(int id, string name)
        {
            InitializeComponent();

            //Метод для добавления к каждой операции привязки данных метода сокрытия служебного столбца с первичным ключом
            dataGridViewCorrectionCollectivesCompositionFree.DataBindingComplete += (s, ev) => HideServiceColumnsCorrectionCollectives();
            dataGridViewCorrectionCollectivesComposition.DataBindingComplete += (s, ev) => HideServiceColumnsCorrectionCollectives();
            this.Text = "Редактирование коллектива";
            this.id = id;
            mode = 2;
            //this.Height = 111;
            textBoxCorrectionCollectivesName.Text = name;
            labelCorrectionCollectivesID.Text = $"Номер: {id}";
            buttonCorrectionCollectivesAddEmpty.Visible = false;
            buttonCorrectionCollectivesAddEmptyAndFill.Visible = false;
            buttonCorrectionCollectivesCancelUp.Visible = false;

            controller = new Query(AppSetting.ConnStr);
            string where = $"WHERE СК.Код_коллектива = {id}";
            dataGridViewCorrectionCollectivesComposition.DataSource = controller.CompositionOfCollectivesTable(where);
            string whereFree = "WHERE СК.Код_коллектива IS NULL";
            dataGridViewCorrectionCollectivesCompositionFree.DataSource = controller.CompositionOfCollectivesTable(whereFree);
        }

        //Метод для сокрытия от пользователя служебного столбца с первичным ключом
        private void HideServiceColumnsCorrectionCollectives()
        {
            if (dataGridViewCorrectionCollectivesCompositionFree.Columns.Contains("Код_коллектива"))
                dataGridViewCorrectionCollectivesCompositionFree.Columns["Код_коллектива"].Visible = false;

            if (dataGridViewCorrectionCollectivesCompositionFree.Columns.Contains("Код_участника"))
                dataGridViewCorrectionCollectivesCompositionFree.Columns["Код_участника"].Visible = false;

            if (dataGridViewCorrectionCollectivesComposition.Columns.Contains("Код_коллектива"))
                dataGridViewCorrectionCollectivesComposition.Columns["Код_коллектива"].Visible = false;

            if (dataGridViewCorrectionCollectivesComposition.Columns.Contains("Код_участника"))
                dataGridViewCorrectionCollectivesComposition.Columns["Код_участника"].Visible = false;
        }

        private void FormCorrectionCollectives_Load(object sender, EventArgs e)
        {

        }

        //Функция нажатия на конопку "Создать пустой коллектив"
        private void buttonCorrectionCollectivesAddEmpty_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionCollectivesName.Text)) { MessageBox.Show("Ошибка ввода Названия!"); return; }
            string name = textBoxCorrectionCollectivesName.Text.Trim();
            if (controller.IsCollectivesHasCoincidence(name) == false && (mode == 1 || mode == 2))
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            //else if (controller.ModifyCollectives(mode, id, name) > 0)
            //{
            controller.ModifyCollectives(mode, id, name);
                this.DialogResult = DialogResult.OK;
                if (mode == 1) MessageBox.Show($"{name} добавлен!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                this.Close();
            //}
        }

        //Функция нажатия на конопку "Создать пустой коллектив и заполнить его"
        private void buttonCorrectionCollectivesAddEmptyAndFill_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionCollectivesName.Text)) { MessageBox.Show("Ошибка ввода Названия!"); return; }
            string name = textBoxCorrectionCollectivesName.Text.Trim();
            if (controller.IsCollectivesHasCoincidence(name) == false && (mode == 1 || mode == 2))
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            //else if (controller.ModifyCollectives(mode, id, name) > 0)
            //{
            //this.DialogResult = DialogResult.OK;
            controller.ModifyCollectives(mode, 0, name);
                if (mode == 1) MessageBox.Show($"{name} добавлен!");
                labelCorrectionCollectivesID.Visible = true;
                int id = controller.IDCollectivesByName(name);
                labelCorrectionCollectivesID.Text = $"Номер: {id}";
                this.Height = 578;
                textBoxCorrectionCollectivesName.Enabled = false;
                buttonCorrectionCollectivesAddEmpty.Enabled = false;
                buttonCorrectionCollectivesAddEmptyAndFill.Enabled = false;
            buttonCorrectionCollectivesCancelUp.Visible = false;
                string where = $"WHERE СК.Код_коллектива = {id}";
                dataGridViewCorrectionCollectivesComposition.DataSource = controller.CompositionOfCollectivesTable(where);
                string whereFree = "WHERE СК.Код_коллектива IS NULL";
                dataGridViewCorrectionCollectivesCompositionFree.DataSource = controller.CompositionOfCollectivesTable(whereFree);
                //else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                //this.Close();
            //}
        }

        //Функция нажатия на кнопку "Отменить" сверху
        private void buttonCorrectionCollectivesCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Функция нажатия на кнопку "Отменить" сгизу
        private void buttonCorrectionCollectivesCancelDown_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Функция нажатия на конопку "Добавить в коллектив"
        private void buttonCorrectionCollectivesCompositionAddInCollective_Click(object sender, EventArgs e)
        {
            if (dataGridViewCorrectionCollectivesCompositionFree.CurrentRow == null) { MessageBox.Show("Выберите строку для добавления!"); ; return; }
            DataGridViewRow row = dataGridViewCorrectionCollectivesCompositionFree.CurrentRow;
            // Читаем значения из ячеек выбранной строки
            int idCom = Convert.ToInt32(row.Cells["Код_участника"].Value);
            string nameCom = row.Cells["ФИО"].Value.ToString();
            string nameCol = textBoxCorrectionCollectivesName.Text.Trim();
            int idCol = controller.IDCollectivesByName(nameCol);
            if (controller.ModifyCompositionIDCollectives(1, idCom, idCol) > 0)
            {
                //if (mode == 1) MessageBox.Show($"{name} добавлен!");
                MessageBox.Show($"Участник {nameCom} добавлен в коллектив {nameCol}!");
                string where = $"WHERE СК.Код_коллектива = {idCol}";
                dataGridViewCorrectionCollectivesComposition.DataSource = controller.CompositionOfCollectivesTable(where);
                string whereFree = "WHERE СК.Код_коллектива IS NULL";
                dataGridViewCorrectionCollectivesCompositionFree.DataSource = controller.CompositionOfCollectivesTable(whereFree);
            }
        }

        //Функция нажатия на конопку "Удалить из коллектива"
        private void buttonCorrectionCollectivesCompositionDeleteFromCollective_Click(object sender, EventArgs e)
        {
            if (dataGridViewCorrectionCollectivesComposition.CurrentRow == null) { MessageBox.Show("Выберите строку для удаления!"); ; return; }
            DataGridViewRow row = dataGridViewCorrectionCollectivesComposition.CurrentRow;
            // Читаем значения из ячеек выбранной строки
            int idCom = Convert.ToInt32(row.Cells["Код_участника"].Value);
            string nameCom = row.Cells["ФИО"].Value.ToString();
            string nameCol = textBoxCorrectionCollectivesName.Text.Trim();
            int idCol = controller.IDCollectivesByName(nameCol);
            if (controller.ModifyCompositionIDCollectives(2, idCom, idCol) > 0)
            {
                //if (mode == 1) MessageBox.Show($"{name} добавлен!");
                MessageBox.Show($"Участник {nameCom} удалён из коллектива {nameCol}!");
                string where = $"WHERE СК.Код_коллектива = {idCol}";
                dataGridViewCorrectionCollectivesComposition.DataSource = controller.CompositionOfCollectivesTable(where);
                string whereFree = "WHERE СК.Код_коллектива IS NULL";
                dataGridViewCorrectionCollectivesCompositionFree.DataSource = controller.CompositionOfCollectivesTable(whereFree);
            }
        }

        //Функция нажатия на конопку "Подтвердить"
        private void buttonCorrectionCollectivesConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionCollectivesName.Text)) { MessageBox.Show("Ошибка ввода Названия!"); return; }
            string name = textBoxCorrectionCollectivesName.Text.Trim();
            if (controller.IsCollectivesHasCoincidence(name) == false && (mode == 1 || mode == 2))
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            else if (controller.ModifyCollectives(mode, id, name) > 0)
            {
                MessageBox.Show($"Запись {id} отредактирована!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

    }
}
