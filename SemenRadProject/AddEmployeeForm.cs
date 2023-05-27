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
    public partial class AddEmployeeForm : Form
    {

        NpgsqlConnection con;

        public AddEmployeeForm(NpgsqlConnection connection)
        {
            InitializeComponent();
            this.con = connection;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.DialogResult = DialogResult.OK;
            NpgsqlCommand com = new NpgsqlCommand("insert into Employee(firstname, lastname) VALUES (:first_name, :last_name)", this.con);
            com.Parameters.AddWithValue("first_name", textBox1.Text);
            com.Parameters.AddWithValue("last_name", textBox2.Text);
            com.ExecuteNonQuery();
            //this.Parent.update_view();
            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).update_view();
            //Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
