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
using Istomin_KP_Sobitiyka_RKIS_24ISP.Controller;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormMain : Form
    {
        Query controller;

        public FormMain()
        {
            InitializeComponent();
            controller = new Query(AppSetting.ConnStr);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

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
            this.Enabled = true;;

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
            this.Enabled = true; ;
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
            this.Enabled = true; ;
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
            this.Enabled = true; ;
        }

        //Функция выхода из приложения
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            else Close();
        }
    }
}
