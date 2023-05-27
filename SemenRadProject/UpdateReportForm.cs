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
    public partial class UpdateReportForm : Form
    {
        NpgsqlConnection con;

        Dictionary<int, int> employees_ids = new Dictionary<int, int>();

        Dictionary<string, string> tax_dict = new Dictionary<string, string>
        {
            ["Нужен"] = "True",
            ["Не нужен"] = "False"
        };

        int id;
        string[] data;

        public UpdateReportForm(NpgsqlConnection connection, int report_id)
        {
            InitializeComponent();
            this.con = connection;
            this.id = report_id;
            List<string> employees = loadEmployeesData();
            data = loadData(report_id);
            initEmployeesData(employees);
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
            string sql = "SELECT * FROM Report WHERE report_id = " + id.ToString();
            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);

            NpgsqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tb_idx = 0;
                foreach (string col in new string[] { "employee_id", "tax", "report_date" })
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

            comboBox2.Items.Add("Нужен");
            comboBox2.Items.Add("Не нужен");
        }

        private void initData()
        {
            int temp_id = -1;
            foreach (var employee in employees_ids)
            {
                if (employee.Value == int.Parse(data[0]))
                {
                    temp_id = employee.Key;
                }
            }
            comboBox1.SelectedIndex = temp_id;

            if (data[1] == "True")
                comboBox2.SelectedIndex = 0;
            else
                comboBox2.SelectedIndex = 1;

            dateTimePicker1.Text = data[2];
        }

        private void UpdateReportForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlCommand com = new NpgsqlCommand("UPDATE Report SET (employee_id, tax, report_date) = (:employee_id, :tax, :report_date) WHERE report_id = " + id + ";", this.con);
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
    }
}
