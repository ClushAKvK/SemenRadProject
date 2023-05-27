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
    public partial class AddNewCostItemIntoReportForm : Form
    {
        Dictionary<int, int> costItems_ids = new Dictionary<int, int>();

        NpgsqlConnection con;
        int id;

        public AddNewCostItemIntoReportForm(NpgsqlConnection connection, int report_id)
        {
            InitializeComponent();
            this.con = connection;
            this.id = report_id;
            List<string> costItems = loadCostItemsData();
            initData(costItems);
        }

        private List<string> loadCostItemsData()
        {
            string sql = "SELECT * FROM Cost_item;";
            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);
            NpgsqlDataReader reader = com.ExecuteReader();
            List<string> costItems = new List<string>();

            // т.к. id в combo_box начинаются с 0, будем привязывать последовательно эти id-Combo_box к id-клиентов
            int costItem_id = 0;
            while (reader.Read())
            {
                // это происходит здесь. Т.е. наполняем словарь client_ids
                costItems_ids.Add(costItem_id, int.Parse(reader["cost_item_id"].ToString()));
                costItem_id++;
                // парсим запись и соединяем в строку с ФИО
                string client = reader["description"] + ": " + reader["cost"];
                costItems.Add(client);
            }
            reader.Close();
            return costItems;
        }

        private void initData(List<string> costItems)
        {
            foreach (string costItem in costItems)
            {
                comboBox1.Items.Add(costItem);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand com = new NpgsqlCommand("insert into Report_info(report_id, cost_item_id, amount) values (:report_id, :cost_item_id, :amount)", this.con);
            //MessageBox.Show(client_cb.SelectedIndex.ToString());
            com.Parameters.AddWithValue("report_id", this.id);
            com.Parameters.AddWithValue("cost_item_id", costItems_ids[comboBox1.SelectedIndex]);
            com.Parameters.AddWithValue("amount", int.Parse(textBox1.Text));
            com.ExecuteNonQuery();
            //Close();
            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).selectReportInfo(id);
        }
    }
}
