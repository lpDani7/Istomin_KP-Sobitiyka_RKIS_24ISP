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
    public partial class FormCorrectionPreferentialCategories : Form
    {
        Query controller;
        int mode = -1;
        private int id;
        private string name;
        private int concession;

        //Метод инициализации формы Добавления Льготных категорий
        public FormCorrectionPreferentialCategories()
        {
            InitializeComponent();
            mode = 1;
            controller = new Query(AppSetting.ConnStr);
            this.Text = "Добавление Льготных категорий";
            labelCorrectionPreferentialCategoriesID.Text = "";
            textBoxCorrectionPreferentialCategoriesName.Clear();
            textBoxCorrectionPreferentialCategoriesConcession.Clear();
        }

        //Метод инициализации формы Редактирования Льготных категорий
        public FormCorrectionPreferentialCategories(int id, string name, int concession)
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            mode = 2;
            this.Text = "Редактирование Льготных категорий";
            //Исходные значения
            this.id = id;
            this.name = name;
            this.concession = concession;
            //Заполение формы редактированиия значениями из выбранной пользователем строки
            labelCorrectionPreferentialCategoriesID.Text = $"Номер: {id}";
            textBoxCorrectionPreferentialCategoriesName.Text = name;
            textBoxCorrectionPreferentialCategoriesConcession.Text = concession.ToString();
        }

        //Метод кнопки "Подтвердить"
        private void buttonCorrectionPreferentialCategoriesConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionPreferentialCategoriesName.Text)) { MessageBox.Show("Ошибка ввода Названия категории!"); return; }
            else if (String.IsNullOrWhiteSpace(textBoxCorrectionPreferentialCategoriesConcession.Text)) { MessageBox.Show("Ошибка ввода Послабления(%)!"); return; }
            string name = textBoxCorrectionPreferentialCategoriesName.Text.Trim();
            int concession = Convert.ToInt32(textBoxCorrectionPreferentialCategoriesConcession.Text.Trim());
            if (controller.IsPreferentialCategoriesHasCoincidence(name, concession) == false && (mode == 1 || mode ==2))
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            else if (controller.ModifyPreferentialCategories(mode, id, name, concession) > 0)
            {
                this.DialogResult = DialogResult.OK;
                if (mode == 1) MessageBox.Show($"{name}, {concession} добавлена!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                this.Close();
            }
        }

        //Метод кнопки "Отмена"
        private void buttonCorrectionPreferentialCategoriesCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Событие обработки ввода текста в textBoxCorrectionPreferentialCategoriesConcession
        private void textBoxCorrectionPreferentialCategoriesConcession_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxCorrectionPreferentialCategoriesConcession.Text.Length > 5) e.KeyChar = (char)0;
        }
    }
}
