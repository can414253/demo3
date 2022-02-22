using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)

        {
            [WebMethod]
            public static List<object> GetCustomers()
            {
                List<object> customers = new List<object>();
                string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                string query = "SELECT * FROM Customers";
                SqlCommand cmd = new SqlCommand(query);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        customers.Add(new
                        {
                            Id = Convert.ToInt32(sdr["CustomerId"]),
                            Name = sdr["Name"].ToString(),
                            Country = sdr["Country"].ToString()
                        });
                    }
                    con.Close();
                }

                return customers;
            }

            [WebMethod]
            public static void AddUpdateCustomer(int id, string name, string country)
            {
                string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                string query = "";
                SqlCommand cmd = new SqlCommand();
                if (id > 0)
                {
                    query = "UPDATE Customers SET Name = @Name,Country = @Country WHERE CustomerId = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Country", country);
                }
                else
                {
                    query = "INSERT INTO Customers VALUES (@Name, @Country)";
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Country", country);
                }

                using (SqlConnection con = new SqlConnection(conString))
                {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            [WebMethod]
            public static void DeleteCustomer(int id)
            {
                string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                string query = "DELETE FROM Customers WHERE CustomerId = @Id";
                SqlCommand cmd = new SqlCommand(query);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
