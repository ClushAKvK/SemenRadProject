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
    public partial class UpdateCostItemForm : Form
    {
        NpgsqlConnection con;

        int costitem_id;
        string[] data;

        public UpdateCostItemForm(NpgsqlConnection connection, int costitem_id)
        {
            InitializeComponent();
            this.con = connection;
            this.data = loadData(costitem_id);
            this.costitem_id = costitem_id;
            initData();
        }

        private string[] loadData(int id)
        {
            data = new string[2];
            string sql = "SELECT * FROM Cost_item WHERE cost_item_id = " + id.ToString();
            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);

            NpgsqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tb_idx = 0;
                foreach (string col in new string[] { "description", "cost" })
                {
                    data[tb_idx] = reader[col].ToString();
                    tb_idx++;
                }
            }
            reader.Close();

            return data;
        }

        private void initData()
        {
            textBox1.Text = data[0];
            textBox2.Text = data[1];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand com = new NpgsqlCommand(@"UPDATE Cost_item SET (description, cost) = (:description, :cost) WHERE cost_item_id = :id", this.con);
            com.Parameters.AddWithValue("description", textBox1.Text);
            com.Parameters.AddWithValue("cost", int.Parse(textBox2.Text));
            com.Parameters.AddWithValue("id", this.costitem_id);
            com.ExecuteNonQuery();

            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).update_view();
        }
    }
}
