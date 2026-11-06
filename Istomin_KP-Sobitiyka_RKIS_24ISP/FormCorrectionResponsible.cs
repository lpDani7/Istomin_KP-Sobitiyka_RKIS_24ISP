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
    public partial class FormCorrectionResponsible : Form
    {
        Query controller;
        int mode = -1;
        private int id;
        private string name;
        private string post;
        private DateTime Date;
        private string gender;

        //Метод инициализации формы Добавления Ответственного
        public FormCorrectionResponsible()
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            comboBoxCorrectionResponsibleGender.Items.Add("Мужской");
            comboBoxCorrectionResponsibleGender.Items.Add("Женский");
            this.Text = "Добавление Ответственных";
            mode = 1;
            comboBoxCorrectionResponsibleGender.SelectedIndex = -1;
            labelCorrectionResponsibleID.Text = "";
            dateTimePickerCorrectionResponsibleDate.Value = DateTime.Now;
            textBoxCorrectionResponsibleName.Clear();
            textBoxCorrectionResponsiblePost.Clear();
        }

        //Метод инициализации формы Редактирования Ответственного
        public FormCorrectionResponsible(int id, string name, string post, DateTime Date, string gender)
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            comboBoxCorrectionResponsibleGender.Items.Add("Мужской");
            comboBoxCorrectionResponsibleGender.Items.Add("Женский");
            mode = 2;
            this.Text = "Редактирование Ответственных";
            //Исходные значения
            this.id = id;
            this.name = name;
            this.post = post;
            this.Date = Date;
            this.gender = gender;
            //Заполение формы редактированиия значениями из выбранной пользователем строки
            labelCorrectionResponsibleID.Text = $"Номер: {id}";
            textBoxCorrectionResponsibleName.Text = name;
            textBoxCorrectionResponsiblePost.Text = post;
            dateTimePickerCorrectionResponsibleDate.Value = Date;
            comboBoxCorrectionResponsibleGender.SelectedItem = gender;
        }

        //Метод кнопки "Отмена"
        private void buttonCorrectionResponsibleCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Метод кнопки "Подтвердить"
        private void buttonCorrectionResponsibleConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionResponsibleName.Text)) { MessageBox.Show("Ошибка ввода ФИО!"); return; }
            else if (String.IsNullOrWhiteSpace(textBoxCorrectionResponsiblePost.Text)) { MessageBox.Show("Ошибка ввода Должности!"); return; }
            else if (comboBoxCorrectionResponsibleGender.SelectedIndex==-1) { MessageBox.Show("Выберите пол!"); return; }
            string name = textBoxCorrectionResponsibleName.Text.Trim();
            string post = textBoxCorrectionResponsiblePost.Text.Trim();
            string Date = dateTimePickerCorrectionResponsibleDate.Value.ToString("yyyy-MM-dd");
            string gender = comboBoxCorrectionResponsibleGender.SelectedItem.ToString();
            if (controller.IsResponsibleHasCoincidence(name, post, Date, gender)==false && mode == 1)
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            else if (controller.ModifyResponseble(mode, id, name, post, dateTimePickerCorrectionResponsibleDate.Value, gender) > 0)
            {
                this.DialogResult = DialogResult.OK;
                if (mode == 1) MessageBox.Show($"{name}, {post} добавлен(а)!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                this.Close();
            }
        }

        //Событие обработки ввода текста в textBoxCorrectionResponsibleName
        private void textBoxCorrectionResponsibleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) e.Handled = true; //Запрет на ввод цифр, сохранение ввода Backspace, Delete и Enter
            if (textBoxCorrectionResponsibleName.Text.Length > 100) e.KeyChar = (char)0;
            // e.KeyChar = (char)0; - Запрет ввода со звуком ошибки
            // e.Handled = true; - Без звука
        }

        //Событие обработки ввода текста в textBoxCorrectionResponsiblePost
        private void textBoxCorrectionResponsiblePost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) e.Handled = true; //Запрет на ввод цифр, сохранение ввода Backspace, Delete и Enter
            if (textBoxCorrectionResponsiblePost.Text.Length > 100) e.KeyChar = (char)0;
        }
    }

}
