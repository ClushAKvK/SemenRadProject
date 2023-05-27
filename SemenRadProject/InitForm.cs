using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemenRadProject
{
    public partial class InitForm : Form
    {


        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        NpgsqlConnection con;

        string current_table = "";

        Form tempSubForm;
        Form report_info;

        public InitForm()
        {
            InitializeComponent();
            this.con = new NpgsqlConnection(
                    "Server=localhost; Port=5432; Username=postgres; Password=2305; database=SemenRadProject"
                );
            con.Open();
            //update_view();

        }

        public void update_view()
        {

            if (true)
            {
                string sql = "SELECT * FROM " + current_table + ";";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, this.con);
                ds.Reset();
                da.Fill(ds);
                dt = ds.Tables[0];
                dataGridView1.DataSource = dt;

                //if (current_table == "Client")
                //{
                //    string[] coColumns = { "Код", "Имя", "Фамилия" };
                //    foreach (DataGridViewColumn col in dataGridView1.Columns)
                //    {
                //        col.HeaderText = coColumns[col.Index];
                //    }
                //}
                //else
                //{
                //    string[] coColumns = { "Идентификатор", "Название", "Описание", "Ед. измерения", "Цена" };
                //    foreach (DataGridViewColumn col in dataGridView1.Columns)
                //    {
                //        col.HeaderText = coColumns[col.Index];
                //    }
                //}
            }
            //else if (current_table == "Contract")
            //{
            //    string sql = @"SELECT ct.contract_id, cl.last_name as client, ct.pay_type, ct.status, ct.register_date, ct.total_price 
            //                    FROM Contract ct
            //                    JOIN Client cl ON ct.client_id = cl.client_id;";
            //    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, this.con);
            //    ds.Reset();
            //    da.Fill(ds);
            //    dt = ds.Tables[0];
            //    dataGridView1.DataSource = dt;

            //    string[] coColumns = { "Идентификатор", "Клиент", "Тип оплаты", "Статус", "День регистрации", "Итоговая сумма" };
            //    foreach (DataGridViewColumn col in dataGridView1.Columns)
            //    {
            //        col.HeaderText = coColumns[col.Index];
            //    }

            //    dataGridView1.Size = new Size(833, 250);
            //}
            //else if (current_table == "Contract_Goods")
            //{
            //    string sql = @"SELECT cg.contract_goods_id, cg.contract_id, go.title as goods, cg.amount, cg.price 
            //                    FROM Contract_Goods cg
            //                    JOIN Goods go ON go.goods_id = cg.goods_id;";
            //    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, this.con);
            //    ds.Reset();
            //    da.Fill(ds);
            //    dt = ds.Tables[0];
            //    dataGridView1.DataSource = dt;
            //    dataGridView1.Columns["contract_goods_id"].DisplayIndex = 0;
            //    dataGridView1.Size = new Size(833, 511);

            //    string[] coColumns = { "Идентификатор", "Договор", "Название товара", "Количество", "Итоговая сумма" };
            //    foreach (DataGridViewColumn col in dataGridView1.Columns)
            //    {
            //        col.HeaderText = coColumns[col.Index];
            //    }
            //}

            dataGridView1.Sort(dataGridView1.Columns[current_table.ToLower() + "_id"], ListSortDirection.Ascending);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (current_table == "Report") {
                tableLayoutPanel1.SetRowSpan(dataGridView1, 4);
                dataGridView2.Visible = false;
                button5.Visible = false;
                report_info.Close();
            }

            this.current_table = "Employee";
            this.update_view();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (current_table == "Report")
            {
                tableLayoutPanel1.SetRowSpan(dataGridView1, 4);
                dataGridView2.Visible = false;
                button5.Visible = false;
                report_info.Close();
            }

            this.current_table = "Cost_item";
            this.update_view();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (current_table == "Report")
            {
                tableLayoutPanel1.SetRowSpan(dataGridView1, 4);
                dataGridView2.Visible = false;
                button5.Visible = false;
                report_info.Close();
            }

            this.current_table = "Sub_report";
            this.update_view();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.current_table = "Report";
            tableLayoutPanel1.SetRowSpan(dataGridView1, 2);
            initCostItemForReport();
            this.update_view();
            initReportInfoForm();
            button5.Visible = true;
        }

        private void initCostItemForReport() {
            tableLayoutPanel1.SetCellPosition(dataGridView2, new TableLayoutPanelCellPosition(0, 4));
            tableLayoutPanel1.SetColumnSpan(dataGridView2, 4);
            tableLayoutPanel1.SetRowSpan(dataGridView2, 2);
            dataGridView2.Size = new Size(960, 565);

            dataGridView2.Visible = true;
        }

        private void initReportInfoForm() {
            if (report_info != null)
                report_info.Close();

            int id = (int)dataGridView1.CurrentRow.Cells[current_table + "_id"].Value;
            report_info = new AddNewCostItemIntoReportForm(con, id);
            report_info.TopLevel = false;
            Controls.Add(report_info);
            tableLayoutPanel1.Controls.Add(report_info, 4, 4);
            report_info.Show();
        }

        public void selectReportInfo(int id) {
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string sql = "SELECT ri.* FROM Report r " +
                            "JOIN Report_info ri ON ri.report_id = r.report_id " +
                            "JOIN Cost_item ci ON ci.cost_item_id = ri.cost_item_id " +
                            "WHERE r.report_id = " + id + ";";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, this.con);
            ds1.Reset();
            da.Fill(ds1);
            dt1 = ds1.Tables[0];
            dataGridView2.DataSource = dt1;
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tempSubForm != null)
                tempSubForm.Close();

            if (current_table == "Employee")
            {
                tempSubForm = new AddEmployeeForm(this.con);
            }
            else if (current_table == "Cost_item")
            {
                tempSubForm = new AddCostItemForm(this.con);
            }
            else if (current_table == "Sub_report") {
                tempSubForm = new AddSubReportForm(this.con);
            }
            else if (current_table == "Report") {
                tempSubForm = new AddReportForm(this.con);
            }

            tempSubForm.TopLevel = false;
            Controls.Add(tempSubForm);
            tableLayoutPanel1.Controls.Add(tempSubForm, 5, 2);
            tempSubForm.Show();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tempSubForm != null)
                tempSubForm.Close();

            int id = (int)dataGridView1.CurrentRow.Cells[current_table + "_id"].Value;

            if (current_table == "Employee")
            {
                tempSubForm = new UpdateEmployeeForm(this.con, id);
            }
            else if (current_table == "Cost_item") {
                tempSubForm = new UpdateCostItemForm(this.con, id);
            }
            else if (current_table == "Sub_report")
            {
                tempSubForm = new UpdateSubReportForm(this.con, id);
            }
            else if (current_table == "Report")
            {
                tempSubForm = new UpdateReportForm(this.con, id);
            }

            tempSubForm.TopLevel = false;
            Controls.Add(tempSubForm);
            tableLayoutPanel1.Controls.Add(tempSubForm, 5, 2);
            tempSubForm.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (current_table == "Report") {
                int id = (int)dataGridView1.CurrentRow.Cells[current_table + "_id"].Value;
                selectReportInfo(id);
                initReportInfoForm();
                //update_view();
            }
            редактироватьToolStripMenuItem1_Click(sender, new EventArgs());
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[current_table + "_id"].Value;
            NpgsqlCommand com = new NpgsqlCommand("DELETE FROM " + current_table + " WHERE " + current_table + "_id = " + id + ";", this.con);
            com.Parameters.AddWithValue("id", id);
            com.ExecuteNonQuery();
            update_view();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            int id = (int)dataGridView2.CurrentRow.Cells["report_info_id"].Value;
            NpgsqlCommand com = new NpgsqlCommand("DELETE FROM Report_info WHERE report_info_id = " + id + ";", this.con);
            com.Parameters.AddWithValue("id", id);
            com.ExecuteNonQuery();

            id = (int)dataGridView1.CurrentRow.Cells[current_table + "_id"].Value;
            selectReportInfo(id);
        }

        private void создатьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeReport form = new MakeReport(this.con);
            form.ShowDialog();

        }
    }
}
