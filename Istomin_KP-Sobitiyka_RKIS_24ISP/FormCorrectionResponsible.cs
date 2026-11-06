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
            /*try {*/ string name = textBoxCorrectionResponsibleName.Text.Trim();/* } catch { MessageBox.Show("Ошибка ввода ФИО!");}*/
            /*try {*/ string post = textBoxCorrectionResponsiblePost.Text.Trim(); /*} catch { MessageBox.Show("Ошибка ввода Должности!"); return; }*/
            string Date = dateTimePickerCorrectionResponsibleDate.Value.ToString("yyyy-MM-dd");
            /*try {*/ string gender = comboBoxCorrectionResponsibleGender.SelectedItem.ToString(); /*} catch { MessageBox.Show("Ошибка ввода Пола!"); return; }*/
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
            //dataGridView.DataSource = controller.TableTovar();
        }
    }

}
