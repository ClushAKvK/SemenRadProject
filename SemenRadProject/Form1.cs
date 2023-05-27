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
    public partial class Form1 : Form
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        NpgsqlConnection con;

        string current_table = "";

        public Form1()
        {
            InitializeComponent();

            this.con = new NpgsqlConnection(
                    "Server=localhost; Port=5432; Username=postgres; Password=2305; database=SemenRadProject"
                );
            con.Open();
        }

        private void update_view()
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

            if (current_table == "Employee") {
                AddEmployeeForm form = new AddEmployeeForm(this.con);
                form.TopLevel = false;
                Controls.Add(form);
                form.Show();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.current_table = "Employee";
            this.update_view();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.current_table = "Cost_item";
            this.update_view();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.current_table = "Sub_report";
            this.update_view();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.current_table = "Report";
            this.update_view();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
