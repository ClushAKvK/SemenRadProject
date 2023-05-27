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
    public partial class AddCostItemForm : Form
    {

        NpgsqlConnection con;

        public AddCostItemForm(NpgsqlConnection con)
        {
            InitializeComponent();
            this.con = con;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.DialogResult = DialogResult.OK;
            NpgsqlCommand com = new NpgsqlCommand("insert into Cost_item(description, cost) VALUES (:description, :cost)", this.con);
            com.Parameters.AddWithValue("description", textBox1.Text);
            com.Parameters.AddWithValue("cost", int.Parse(textBox2.Text));
            com.ExecuteNonQuery();
            //this.Parent.update_view();
            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).update_view();
        }
    }
}
