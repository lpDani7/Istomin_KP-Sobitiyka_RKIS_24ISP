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

        private void ответственныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(0);
            formSprav.Show();

        }

        private void местаПроведенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(1);
            formSprav.Show();
        }

        private void типыРасходовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(2);
            formSprav.Show();
        }

        private void льготныеКатегорииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSprav formSprav = new FormSprav(3);
            formSprav.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            else Close();
        }
    }
}
