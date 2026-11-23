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
using System.Windows.Forms.DataVisualization.Charting;

namespace Istomin_KP_Sobitiyka_RKIS_24ISP
{
    public partial class FormReport : Form
    {
        Reports controller;
        public FormReport()
        {
            InitializeComponent();
            controller = new Reports(AppSetting.ConnStr);

            FillReportComboBox();
            FillReportTable();
            FillReportChart();
            /*comboBoxReportCountEventSelectYear.SelectedIndex = 0;
            comboBoxReportTrafficSelectYear.SelectedIndex = 0;
            comboBoxReportTypeTrafficSelectYear.SelectedIndex = 0;
            comboBoxReportExpensesSelectYear.SelectedIndex = 0;
            comboBoxReportIncomeSelectYear.SelectedIndex = 0;
            comboBoxReportTypeTrafficSelectMonth.SelectedIndex = 0;*/
        }

        //Функция заполнения таблиц отчётов
        private void FillReportComboBox()
        {
            //заполнение comboBox
            comboBoxReportCountEventSelectYear.DataSource = controller.YearTable();
            comboBoxReportCountEventSelectYear.ValueMember = "Год";
            comboBoxReportTrafficSelectYear.DataSource = controller.YearTable();
            comboBoxReportTrafficSelectYear.ValueMember = "Год";
            comboBoxReportTypeTrafficSelectYear.DataSource = controller.YearTable();
            comboBoxReportTypeTrafficSelectYear.ValueMember = "Год";
            comboBoxReportExpensesSelectYear.DataSource = controller.YearTable();
            comboBoxReportExpensesSelectYear.ValueMember = "Год";
            comboBoxReportIncomeSelectYear.DataSource = controller.YearTable();
            comboBoxReportIncomeSelectYear.ValueMember = "Год";
            string where = $" WHERE strftime('%Y', М.Дата) like '{comboBoxReportTypeTrafficSelectYear.SelectedValue.ToString()}'";
            comboBoxReportTypeTrafficSelectMonth.DataSource = controller.MonthTable(where);
            comboBoxReportTypeTrafficSelectMonth.ValueMember = "Месяц";
        }

        //Функция заполнения таблиц отчётов
        private void FillReportTable()
        {
            //заполнение таблиц отчётов
            string having = $" HAVING strftime('%Y', Дата) like '{comboBoxReportCountEventSelectYear.SelectedValue.ToString()}'";
            dataGridViewReportCountEvent.DataSource = controller.CountEventTable(having);
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportTrafficSelectYear.SelectedValue.ToString()}'";
            dataGridViewReportTraffic.DataSource = controller.TrafficTable(having);
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportTypeTrafficSelectYear.SelectedValue.ToString()}' " +
                     $"and strftime('%m', М.Дата) like '{comboBoxReportTypeTrafficSelectMonth.SelectedValue.ToString()}'";
            dataGridViewReportTypeTraffic.DataSource = controller.TypeTrafficTable(having);
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportExpensesSelectYear.SelectedValue.ToString()}'";
            dataGridViewReportExpenses.DataSource = controller.ExpensesTable(having);
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportIncomeSelectYear.SelectedValue.ToString()}'";
            dataGridViewReportIncome.DataSource = controller.IncomeTable(having);

        }

        //Функция заполнения таблиц отчётов
        private void FillReportChart()
        {
            //заполнение графиков
            string having = $" HAVING strftime('%Y', Дата) like '{comboBoxReportCountEventSelectYear.SelectedValue.ToString()}'";
            chartReportCountEvent.DataSource = controller.CountEventTable(having);
            chartReportCountEvent.Series[0].XValueMember = "Месяц";
            chartReportCountEvent.Series[0].YValueMembers = "Количество мероприятий";
            chartReportCountEvent.DataBind();
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportTrafficSelectYear.SelectedValue.ToString()}'";
            chartReportTraffic.DataSource = controller.TrafficTable(having);
            chartReportTraffic.Series[0].XValueMember = "Месяц";
            chartReportTraffic.Series[0].YValueMembers = "Не льготная посещаемость";
            chartReportTraffic.Series[1].XValueMember = "Месяц";
            chartReportTraffic.Series[1].YValueMembers = "Льготная посещаемость";
            chartReportTraffic.Legends[0].TextWrapThreshold = 5;
            chartReportTraffic.DataBind();
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportTypeTrafficSelectYear.SelectedValue.ToString()}' " +
                     $"and strftime('%m', М.Дата) like '{comboBoxReportTypeTrafficSelectMonth.SelectedValue.ToString()}'";
            chartReportTypeTraffic.DataSource = controller.TypeTrafficTable(having);
            chartReportTypeTraffic.Series[0].XValueMember = "Льготная категория";
            chartReportTypeTraffic.Series[0].YValueMembers = "Количество";
            chartReportTypeTraffic.DataBind();
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportExpensesSelectYear.SelectedValue.ToString()}'";
            chartReportExpenses.DataSource = controller.ExpensesTable(having);
            chartReportExpenses.Series[0].XValueMember = "Месяц";
            chartReportExpenses.Series[0].YValueMembers = "Расход";
            chartReportExpenses.DataBind();
            having = $" HAVING strftime('%Y', М.Дата) like '{comboBoxReportIncomeSelectYear.SelectedValue.ToString()}'";
            chartReportIncome.DataSource = controller.IncomeTable(having);
            chartReportIncome.Series[0].XValueMember = "Месяц";
            chartReportIncome.Series[0].YValueMembers = "Доход";
            chartReportIncome.DataBind();
        }

        //Функция обновления данных формы при изменении года для Количества мероприятияй
        private void comboBoxReportCountEventSelectYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillReportTable();
            FillReportChart();
        }

        //Функция обновления данных формы при изменении года для Посещаемости
        private void comboBoxReportTrafficSelectYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillReportTable();
            FillReportChart();
        }

        //Функция обновления списка месяцев при изменении года для Типов посещаемости
        private void comboBoxReportTypeTrafficSelectYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string where = $" WHERE strftime('%Y', М.Дата) like '{comboBoxReportTypeTrafficSelectYear.SelectedValue.ToString()}'";
            comboBoxReportTypeTrafficSelectMonth.DataSource = controller.MonthTable(where);
            comboBoxReportTypeTrafficSelectMonth.ValueMember = "Месяц";
            FillReportTable();
            FillReportChart();
        }

        //Функция обновления данных формы при изменении месяца для Типов посещаемости
        private void comboBoxReportTypeTrafficSelectMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillReportTable();
            FillReportChart();
        }

        //Функция обновления данных формы при изменении года для Расходов
        private void comboBoxReportExpensesSelectYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillReportTable();
            FillReportChart();
        }

        //Функция обновления данных формы при изменении года для Доходов
        private void comboBoxReportIncomeSelectYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FillReportTable();
            FillReportChart();
        }
    }
}
