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
    public partial class UpdateSubReportForm : Form
    {
        NpgsqlConnection con;

        Dictionary<int, int> employees_ids = new Dictionary<int, int>();

        int id;
        string[] data;

        public UpdateSubReportForm(NpgsqlConnection connection, int sub_report_id)
        {
            InitializeComponent();
            this.con = connection;
            this.id = sub_report_id;
            List<string> employees = loadEmployeesData();
            initEmployeesData(employees);
            data = loadData(sub_report_id);
            initData();
        }

        private List<string> loadEmployeesData()
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

        private string[] loadData(int id)
        {
            data = new string[3];
            string sql = "SELECT * FROM Sub_report WHERE sub_report_id = " + id.ToString();
            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);

            NpgsqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tb_idx = 0;
                foreach (string col in new string[] { "employee_id", "sum", "sub_report_date" })
                {
                    data[tb_idx] = reader[col].ToString();
                    tb_idx++;
                }
            }
            reader.Close();

            return data;
        }

        private void initEmployeesData(List<string> employees)
        {
            foreach (string employee in employees)
            {
                comboBox1.Items.Add(employee);
            }
        }

        private void initData() {
            int temp_id = -1;
            foreach (var employee in employees_ids) {
                if (employee.Value == int.Parse(data[0])) {
                    temp_id = employee.Key;
                }
            }
            comboBox1.SelectedIndex = temp_id;
            textBox2.Text = data[1];
            dateTimePicker1.Text = data[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand com = new NpgsqlCommand("UPDATE Sub_report SET (employee_id, sum, sub_report_date) = (:employee_id, :sum, :sub_report_date) WHERE sub_report_id = " + id + ";", this.con);
            //MessageBox.Show(client_cb.SelectedIndex.ToString());
            com.Parameters.AddWithValue("employee_id", employees_ids[comboBox1.SelectedIndex]);
            com.Parameters.AddWithValue("sum", int.Parse(textBox2.Text));
            NpgsqlParameter date1 = new NpgsqlParameter("sub_report_date", NpgsqlTypes.NpgsqlDbType.Date);
            date1.Value = dateTimePicker1.Value.Date;
            com.Parameters.Add(date1);
            com.ExecuteNonQuery();
            //Close();
            (System.Windows.Forms.Application.OpenForms["InitForm"] as InitForm).update_view();
        }
    }
}
