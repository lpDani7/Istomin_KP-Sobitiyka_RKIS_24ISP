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
    public partial class FormCorrectionPlaces : Form
    {
        Query controller;
        int mode = -1;
        private int id;
        private string name;
        private string adress;
        private int capacity;

        //Метод инициализации формы Добавления Метса проведения
        public FormCorrectionPlaces()
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            this.Text = "Добавление Мест проведения";
            mode = 1;
            labelCorrectionPlacesID.Text = "";
            textBoxCorrectionPlacesName.Clear();
            textBoxCorrectionPlacesAdress.Clear();
            textBoxCorrectionPlacesCapacity.Clear();
        }

        //Метод инициализации формы Редактирования Метса проведения
        public FormCorrectionPlaces(int id, string name, string adress, int capacity)
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
            mode = 2;
            this.Text = "Редактирование Мест проведения";
            //Исходные значения
            this.id = id;
            this.name = name;
            this.adress = adress;
            this.capacity = capacity;
            //Заполение формы редактированиия значениями из выбранной пользователем строки
            labelCorrectionPlacesID.Text = $"Номер: {id}";
            textBoxCorrectionPlacesName.Text = name;
            textBoxCorrectionPlacesAdress.Text = adress;
            textBoxCorrectionPlacesCapacity.Text = capacity.ToString();
        }

        //Метод кнопки "Подтвердить"
        private void buttonCorrectionPlacesConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxCorrectionPlacesName.Text)) { MessageBox.Show("Ошибка ввода Названия!"); return; }
            else if (String.IsNullOrWhiteSpace(textBoxCorrectionPlacesAdress.Text)) { MessageBox.Show("Ошибка ввода Адреса!"); return; }
            else if (String.IsNullOrWhiteSpace(textBoxCorrectionPlacesCapacity.Text)) { MessageBox.Show("Ошибка ввода Вместимости!"); return; }
            string name = textBoxCorrectionPlacesName.Text.Trim();
            string adress = textBoxCorrectionPlacesAdress.Text.Trim();
            int capacity = Convert.ToInt32(textBoxCorrectionPlacesCapacity.Text.Trim());
            if (controller.IsPlacesHasCoincidence(name, adress, capacity) == false && mode == 1)
            {
                MessageBox.Show("Запись с такими данными уже существует!");
            }
            else if (controller.ModifyPlaces(mode, id, name, adress, capacity) > 0)
            {
                this.DialogResult = DialogResult.OK;
                if (mode == 1) MessageBox.Show($"{name}, {adress} добавлен(о)!");
                else if (mode == 2) MessageBox.Show($"Запись {id} отредактирована!");
                this.Close();
            }
        }

        //Метод кнопки "Отмена"
        private void buttonCorrectionPlacesCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Событие обработки ввода текста в textBoxCorrectionPlacesCapacity
        private void textBoxCorrectionPlacesCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;//Зпрет на ввод любых символов кроме чисел
            if (textBoxCorrectionPlacesCapacity.Text.Length > 5) e.KeyChar = (char)0;
        }

    }
}
