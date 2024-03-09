using S18_compito.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace S18_compito.Controllers
{
    public class CustomersController : Controller
    {
        public ActionResult Index()
        {
            List<Customers> customer = GetCustomers();
            return View(customer);
        }

        private List<Customers> GetCustomers()
        {
            string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();
            List<Customers> customer = new List<Customers>();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                using (SqlCommand c = new SqlCommand(@"SELECT * FROM Customers", conn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                Customers cus = new Customers();
                                cus.Nome = (string)r["Nome"];
                                cus.Cognome = (string)r["Cognome"];
                                cus.CF = (string)r["CF"];
                                cus.Città = (string)r["Città"];
                                cus.Provincia = (string)r["Provincia"];
                                cus.Email = (string)r["Email"];
                                cus.Tel = (int)r["Tel"];
                                customer.Add(cus);
                            }
                        }
                    }
                }
            }
            return customer;
        }
    }
}