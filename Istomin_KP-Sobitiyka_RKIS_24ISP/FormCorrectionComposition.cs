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

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormCorrectionComposition : Form
    {
        int mode = 0;
        private int id;
        string name;
        string post;
        DateTime Date;
        int gender;
        int collective;
        Query controller;


        public FormCorrectionComposition()
        {
            InitializeComponent();

            this.Text = "Добавление участника";
            mode = 1;
            controller = new Query(AppSetting.ConnStr);
            labelCorrectionCompositionID.Visible = false;
            textBoxCorrectionCompositionName.Clear();
            textBoxCorrectionCompositionPost.Clear();
            dateTimePickerCorrectionCompositionDate.Value = DateTime.Now;
            comboBoxCorrectionCompositionGender.Items.Clear();
            comboBoxCorrectionCompositionGender.Items.Add("Мужской");
            comboBoxCorrectionCompositionGender.Items.Add("Женский");
            comboBoxCorrectionCompositionGender.SelectedIndex = -1;
            comboBoxCorrectionCompositionCollectives.Items.Clear();
            comboBoxCorrectionCompositionCollectives.DataSource = controller.CompositionIDAndCollectivesNameTable();
            comboBoxCorrectionCompositionCollectives.ValueMember = "Код_коллектива";
            comboBoxCorrectionCompositionCollectives.DisplayMember = "Название";
            comboBoxCorrectionCompositionCollectives.SelectedIndex = -1;

            /*controller = new Query(AppSetting.ConnStr);
            string where = "WHERE СК.Код_коллектива IS NULL";
            dataGridViewCorrectionCollectivesCompositionFree.DataSource = controller.CompositionOfCollectivesTable(where);*/
        }

        public FormCorrectionComposition(int id, string name, string post, DateTime Date, int gender, int collectiveID)
        {
            InitializeComponent();

            this.Text = "Редактирование участника";
            this.id = id;
            mode = 2;
            controller = new Query(AppSetting.ConnStr);
            labelCorrectionCompositionID.Text = $"Номер: {id}";
            this.name = name;
            textBoxCorrectionCompositionName.Text = name;
            this.post = post;
            textBoxCorrectionCompositionPost.Text = post;
            this.Date = Date;
            dateTimePickerCorrectionCompositionDate.Value = Date;
            comboBoxCorrectionCompositionGender.Items.Add("Мужской");
            comboBoxCorrectionCompositionGender.Items.Add("Женский");
            this.gender = gender;
            comboBoxCorrectionCompositionGender.SelectedIndex = gender;
            comboBoxCorrectionCompositionCollectives.Items.Clear();
            comboBoxCorrectionCompositionCollectives.DataSource = controller.CompositionIDAndCollectivesNameTable();
            comboBoxCorrectionCompositionCollectives.ValueMember = "Код_коллектива";
            comboBoxCorrectionCompositionCollectives.DisplayMember = "Название";
            this.collective = collectiveID;
            if (collectiveID == -10)
            {
                comboBoxCorrectionCompositionCollectives.SelectedIndex = -1;
                checkBoxCorrectionCompositionCollectivesFree.Checked = true;
            }
            else comboBoxCorrectionCompositionCollectives.SelectedValue = collectiveID;
        }

        //Функция нажатия на кнопку "Свободен"
        private void checkBoxCorrectionCompositionCollectivesFree_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCorrectionCompositionCollectivesFree.Checked == true)
            { 
                comboBoxCorrectionCompositionCollectives.SelectedIndex = -1;
                comboBoxCorrectionCompositionCollectives.Enabled = false;
            }
            else comboBoxCorrectionCompositionCollectives.Enabled = true;
        }

        //Функция нажатия на кнопку "Подтвердить"
        private void buttonCorrectionCompositionConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionCompositionName.Text)) { MessageBox.Show("Ошибка ввода ФИО!"); return; }
            else if (String.IsNullOrWhiteSpace(textBoxCorrectionCompositionPost.Text)) { MessageBox.Show("Ошибка ввода Должности!"); return; }
            else if (comboBoxCorrectionCompositionGender.SelectedIndex == -1) { MessageBox.Show("Выберите пол!"); return; }
            else if (comboBoxCorrectionCompositionCollectives.SelectedIndex == -1 && checkBoxCorrectionCompositionCollectivesFree.Checked == false) 
            { MessageBox.Show("Выберите коллектив!"); return; }
            string name = textBoxCorrectionCompositionName.Text.Trim();
            string post = textBoxCorrectionCompositionPost.Text.Trim();
            string Date = dateTimePickerCorrectionCompositionDate.Value.ToString("yyyy-MM-dd");
            string gender = comboBoxCorrectionCompositionGender.SelectedItem.ToString();
            int Igender = comboBoxCorrectionCompositionGender.SelectedIndex;
            int collective = Convert.ToInt32(comboBoxCorrectionCompositionCollectives.SelectedValue);
            bool chek = checkBoxCorrectionCompositionCollectivesFree.Checked;
            if (controller.IsCompositionHasCoincidenceForAdd(name, post, Date, gender) == false && mode == 1)
            {
                MessageBox.Show("Запись с такими данными уже существует!"); return;
            }
            else if (controller.IsCompositionHasCoincidenceForEdit(name, post, Date, gender, collective) == false && mode == 2)
            {
                if (mode == 2 && (name != this.name || post != this.post || Convert.ToDateTime(Date) != this.Date || Igender != this.gender /*|| collective!=this.collective*/)) { MessageBox.Show("Запись с такими данными уже существует!"); return; }
            }
            if (controller.ModifyComposition(mode, id, name, post, dateTimePickerCorrectionCompositionDate.Value, gender, collective, chek) > 0)
            {
                this.DialogResult = DialogResult.OK;
                if (mode == 1 && checkBoxCorrectionCompositionCollectivesFree.Checked == false) MessageBox.Show($"{name}, {post} добавлен(а) в " +
                    $"коллектив {comboBoxCorrectionCompositionCollectives.SelectedItem}!");
                else if (mode == 1) MessageBox.Show($"{name}, {post} добавлен(а) в коллектив!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                this.Close();
            }
        }

        //Функция нажатия на кнопку "Отмена"
        private void buttonCorrectionCompositionCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Событие обработки ввода текста в textBoxCorrectionCompositionName
        private void textBoxCorrectionCompositionName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) e.Handled = true; //Запрет на ввод цифр, сохранение ввода Backspace, Delete и Enter
            if (textBoxCorrectionCompositionName.Text.Length > 100) e.KeyChar = (char)0;
        }
    }
}
