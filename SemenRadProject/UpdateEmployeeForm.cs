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
    public partial class UpdateEmployeeForm : Form
    {
        NpgsqlConnection con;

        int employee_id;
        string[] data;

        public UpdateEmployeeForm(NpgsqlConnection connection, int employee_id)
        {
            InitializeComponent();
            this.con = connection;
            this.data = loadData(employee_id);
            this.employee_id = employee_id;
            initData();
        }

        private string[] loadData(int id) {
            data = new string[2];
            string sql = "SELECT * FROM Employee WHERE employee_id = " + id.ToString();
            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);

            NpgsqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tb_idx = 0;
                foreach (string col in new string[] { "firstname", "lastname" })
                {
                    data[tb_idx] = reader[col].ToString();
                    tb_idx++;
                }
            }
            reader.Close();

            return data;
        }

        private void initData() {
            textBox1.Text = data[0];
            textBox2.Text = data[1];

            
        }

        private void UpdateEmployeeForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand com = new NpgsqlCommand(@"UPDATE Employee SET (firstname, lastname) = (:first_name, :last_name) WHERE employee_id = :id", this.con);
            com.Parameters.AddWithValue("first_name", textBox1.Text);
            com.Parameters.AddWithValue("last_name", textBox2.Text);
            com.Parameters.AddWithValue("id", this.employee_id);
            com.ExecuteNonQuery();

            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).update_view();
            //Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
