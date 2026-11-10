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
    public partial class FormCorrectionTypesOfExpenses : Form
    {
        Query controller;
        int mode = -1;
        private int id;
        private string name;
        //Метод инициализации формы Добавления Типов расходов
        public FormCorrectionTypesOfExpenses()
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            this.Text = "Добавление Типов расходов";
            mode = 1;
            labelCorrectionTypesOfExpensesID.Text = "";
            textBoxCorrectionTypesOfExpensesName.Clear();
        }

        //Метод инициализации формы Редактирования Типов расходов
        public FormCorrectionTypesOfExpenses(int id, string name)
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            mode = 2;
            this.Text = "Редактирование Типов расходов";
            //Исходные значения
            this.id = id;
            this.name = name;
            //Заполение формы редактированиия значениями из выбранной пользователем строки
            labelCorrectionTypesOfExpensesID.Text = $"Номер: {id}";
            textBoxCorrectionTypesOfExpensesName.Text = name;
        }
        
        //Метод кнопки "Подтвердить"
        private void buttonCorrectionTypesOfExpensesConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionTypesOfExpensesName.Text)) { MessageBox.Show("Ошибка ввода Типа расхода!"); return; }
            string name = textBoxCorrectionTypesOfExpensesName.Text.Trim();
            if (controller.IsTypesOfExpensesHasCoincidence(name) == false && (mode == 1 || mode == 2))
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            else if (controller.ModifyTypesOfExpenses(mode, id, name) > 0)
            {
                this.DialogResult = DialogResult.OK;
                if (mode == 1) MessageBox.Show($"{name} добавлено!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                this.Close();
            }
        }

        //Метод кнопки "Отмена"
        private void buttonCorrectionTypesOfExpensesCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Событие обработки ввода текста в textBoxCorrectionTypesOfExpensesName
        private void textBoxCorrectionTypesOfExpensesName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsDigit(e.KeyChar)) e.Handled = true; //Запрет на ввод цифр, сохранение ввода Backspace, Delete и Enter
            if (textBoxCorrectionTypesOfExpensesName.Text.Length > 100) e.KeyChar = (char)0;
        }
    }
}
