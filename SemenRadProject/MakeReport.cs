using Microsoft.Office.Interop.Excel;
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
    public partial class MakeReport : Form
    {

        Dictionary<int, int> employees_ids = new Dictionary<int, int>();

        DataSet ds = new DataSet();
        System.Data.DataTable dt = new System.Data.DataTable();
        NpgsqlConnection con;

        List<string[]> data = new List<string[]>();

        public MakeReport(NpgsqlConnection connection)
        {
            InitializeComponent();
            this.con = connection;
            InitEmployees();
        }

        public void InitEmployees()
        {
            string sql = "SELECT * FROM Employee;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, this.con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            // отдельно ставим на 0 позицию параметр Все, для быстрого выбора ВСЕХ клиентов
            checkedListBox1.Items.Add("Все");
            checkedListBox1.ItemCheck += ItemCheck;
            int clb_employee = 1;
            foreach (DataRow row in dt.Rows)
            {
                var cells = row.ItemArray;
                checkedListBox1.Items.Add(cells[1] + " " + cells[2]);

                // Связка записи в checkedListBox1 и id-клиента
                employees_ids.Add(clb_employee, (int)cells[0]);
                clb_employee++;
            }
        }

        private void ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox lb = sender as CheckedListBox;
            if (e.Index == 0)
            {
                bool flag = e.NewValue == CheckState.Checked ? true : false;
                for (int i = 1; i < lb.Items.Count; i++)
                    lb.SetItemChecked(i, flag);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT e.firstname, e.lastname, sum(r.total_sum) as total_sum FROM Report r
                            JOIN Employee e ON e.employee_id = r.employee_id
                            WHERE r.tax = 'True' and e.employee_id = ANY(:values) and r.report_date >= :start_date and r.report_date <= :end_date
                            GROUP BY e.employee_id;";

            List<int> values = new List<int>();
            foreach (int index in checkedListBox1.CheckedIndices)
                if (!(index is 0))
                    values.Add(employees_ids[index]);


            NpgsqlCommand com = new NpgsqlCommand(sql, this.con);

            com.Parameters.AddWithValue("values", values.ToArray());

            NpgsqlParameter date1 = new NpgsqlParameter("start_date", NpgsqlTypes.NpgsqlDbType.Date);
            date1.Value = dateTimePicker1.Value.Date;
            com.Parameters.Add(date1);

            NpgsqlParameter date2 = new NpgsqlParameter("end_date", NpgsqlTypes.NpgsqlDbType.Date);
            date2.Value = dateTimePicker2.Value;
            com.Parameters.Add(date2);

            NpgsqlDataReader reader = com.ExecuteReader();

            string[] report_columns = new string[] { "firstname", "lastname", "total_sum" };
            while (reader.Read())
            {
                string str = "";
                string[] els = new string[4];
                for (int i = 0; i < report_columns.Length; i++)
                {
                    string el = reader[report_columns[i]].ToString();
                    els[i] = el;
                }
                els[3] = (int.Parse(els[2]) / 100 * 13).ToString();
                els[2] = (int.Parse(els[2])+ int.Parse(els[2]) / 100 * 13).ToString();
                data.Add(els);
            }
            reader.Close();

            make_report();
        }


        private void make_report()
        {
            // вот до 157 просто код из лаб
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string filename = ofd.FileName;
            Microsoft.Office.Interop.Excel.Application excelObject = new Microsoft.Office.Interop.Excel.Application();
            excelObject.Visible = true;
            Workbook wb = excelObject.Workbooks.Open(filename, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            Worksheet wsh = wb.Sheets[1];
            wsh.Columns.AutoFit();

            string[] ru_report_columns = new string[] { "Имя", "Фамилия", "Сумма с налогом", "Налог" };

            // пишем в первую строчку названия колонок
            for (int i = 0; i < ru_report_columns.Length; i++)
            {
                wsh.Cells[1, i + 1] = ru_report_columns[i];
            }

            // потом построчно собранную инфу из data
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    wsh.Cells[i + 2, j + 1] = data[i][j];
                }
            }

            // Дополнительно указываем период, за который был сдела отчет
            wsh.Cells[1, ru_report_columns.Length + 1] = "Период";
            wsh.Cells[2, ru_report_columns.Length + 1] = dateTimePicker1.Value.ToString();
            wsh.Cells[3, ru_report_columns.Length + 1] = dateTimePicker2.Value.ToString();

            wb.Save();
            wb.Close();
        }
    }
}
