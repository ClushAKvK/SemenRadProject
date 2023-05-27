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
    public partial class AddReportForm : Form
    {
        NpgsqlConnection con;

        Dictionary<int, int> employees_ids = new Dictionary<int, int>();

        Dictionary<string, string> tax_dict = new Dictionary<string, string> {
            ["Нужен"] = "True",
            ["Не нужен"] = "False"
        };

        public AddReportForm(NpgsqlConnection connection)
        {
            InitializeComponent();
            this.con = connection;
            List<string> employees = loadData();
            initData(employees);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private List<string> loadData()
        {
            string sql = "SELECT * FROM Employee;";
            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);
            NpgsqlDataReader reader = com.ExecuteReader();
            List<string> employees = new List<string>();

            // т.к. id в combo_box начинаются с 0, будем привязывать последовательно эти id-Combo_box к id-клиентов
            int employee_id = 0;
            while (reader.Read())
            {
                // это происходит здесь. Т.е. наполняем словарь client_ids
                employees_ids.Add(employee_id, int.Parse(reader["employee_id"].ToString()));
                employee_id++;
                // парсим запись и соединяем в строку с ФИО
                string client = reader["firstname"] + " " + reader["lastname"];
                employees.Add(client);
            }
            reader.Close();
            return employees;
        }

        private void initData(List<string> employees)
        {
            foreach (string employee in employees)
            {
                comboBox1.Items.Add(employee);
            }

            comboBox2.Items.Add("Нужен");
            comboBox2.Items.Add("Не нужен");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand com = new NpgsqlCommand("insert into Report(employee_id, tax, report_date) values (:employee_id, :tax, :report_date)", this.con);
            //MessageBox.Show(client_cb.SelectedIndex.ToString());
            com.Parameters.AddWithValue("employee_id", employees_ids[comboBox1.SelectedIndex]);
            com.Parameters.AddWithValue("tax", tax_dict[comboBox2.SelectedItem.ToString()]);
            NpgsqlParameter date1 = new NpgsqlParameter("report_date", NpgsqlTypes.NpgsqlDbType.Date);
            date1.Value = dateTimePicker1.Value.Date;
            com.Parameters.Add(date1);
            com.ExecuteNonQuery();
            //Close();
            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).update_view();
        }

        private void AddReportForm_Load(object sender, EventArgs e)
        {

        }
    }
}
