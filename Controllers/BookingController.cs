using S18_compito.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace S18_compito.Controllers
{
    public class BookingController : Controller
    {
        public ActionResult Index()
        {
            List<Booking> b = Get();
            return View(b);
        }

        private List<Booking> Get()
        {
            string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();
            List<Booking> b = new List<Booking>();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                using (SqlCommand c = new SqlCommand(@"SELECT Booking.*, Customers.Nome, Customers.Cognome  
FROM Booking  
INNER JOIN Customers ON Booking.IDCliente = Customers.ID
", conn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                Booking bk = new Booking();
                                bk.IDCliente = (int)r["IDCliente"];
                                bk.Nome = (string)r["Nome"];
                                bk.Cognome = (string)r["Cognome"];
                                bk.IDStanza = (int)r["IDStanza"];
                                bk.Anno = (int)r["Anno"];
                                bk.NumeroPrenotazione = (int)r["NumeroPrenotazione"];
                                bk.DataPrenotazione = (DateTime)r["DataPrenotazione"];
                                bk.CheckIn = (DateTime)r["CheckIn"];
                                bk.CheckOut = (DateTime)r["CheckOut"];
                                bk.Caparra = (decimal)r["Caparra"];
                                bk.Tariffa = (decimal)r["Tariffa"];
                                bk.PensioneOmezza = (bool)r["PensioneOmezza"];
                                bk.Colazione = (bool)r["Colazione"];
                                b.Add(bk);
                            }
                        }
                    }
                }
            }
            return b;
        }

        public ActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(Customers customer)
        {
            string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();

                string q = @"INSERT INTO Customers (Nome, Cognome, CF, Città, Provincia, Email, Tel)
                                       VALUES (@Nome, @Cognome, @CF , @Città, @Provincia, @Email, @Tel)";

                using (SqlCommand c = new SqlCommand(q, conn))
                {
                    c.Parameters.AddWithValue("@Nome", customer.Nome);
                    c.Parameters.AddWithValue("@Cognome", customer.Cognome);
                    c.Parameters.AddWithValue("@CF", customer.CF);
                    c.Parameters.AddWithValue("@Città", customer.Città);
                    c.Parameters.AddWithValue("@Provincia", customer.Provincia);
                    c.Parameters.AddWithValue("@Email", customer.Email);
                    c.Parameters.AddWithValue("@Tel", customer.Tel);
                    c.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult AddRes()
        {
            ViewBag.CustomerList = GetCustomersDropdown();
            ViewBag.RoomList = GetRoomsDropdown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRes(Booking b)
        {
            if (ModelState.IsValid)
            {
                string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();

                using (SqlConnection conn = new SqlConnection(connect))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO Booking (IDCliente, IDStanza, Anno, NumeroPrenotazione, DataPrenotazione, 
                                      CheckIn, CheckOut, Caparra, Tariffa, PensioneOmezza, Colazione)
                VALUES (@IDCliente, @IDStanza, @Anno, @NumeroPrenotazione, @DataPrenotazione, 
                        @CheckIn, @CheckOut, @Caparra, @Tariffa, @PensioneOmezza, @Colazione);
                SELECT SCOPE_IDENTITY();
            ";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDCliente", b.IDCliente);
                        cmd.Parameters.AddWithValue("@IDStanza", b.IDStanza);
                        cmd.Parameters.AddWithValue("@Anno", b.Anno);
                        cmd.Parameters.AddWithValue("@NumeroPrenotazione", b.NumeroPrenotazione);
                        cmd.Parameters.AddWithValue("@DataPrenotazione", b.DataPrenotazione);
                        cmd.Parameters.AddWithValue("@CheckIn", b.CheckIn);
                        cmd.Parameters.AddWithValue("@CheckOut", b.CheckOut);
                        cmd.Parameters.AddWithValue("@Caparra", b.Caparra);
                        cmd.Parameters.AddWithValue("@Tariffa", b.Tariffa);
                        cmd.Parameters.AddWithValue("@PensioneOmezza", b.PensioneOmezza);
                        cmd.Parameters.AddWithValue("@Colazione", b.Colazione);

                        int newBookingID = Convert.ToInt32(cmd.ExecuteScalar());

                        return RedirectToAction("Details", new { id = newBookingID });
                    }
                }
            }


            ViewBag.CustomerList = GetCustomersDropdown();
            ViewBag.RoomList = GetRoomsDropdown();

            return View(b);
        }

        public ActionResult Details(int id)
        {
            Booking booking = GetBookingById(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        private SelectList GetCustomersDropdown()
        {
            string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                using (SqlCommand c = new SqlCommand(@"SELECT ID, Nome, Cognome FROM Customers", conn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            List<SelectListItem> customers = new List<SelectListItem>();
                            while (r.Read())
                            {
                                customers.Add(new SelectListItem
                                {
                                    Value = r["ID"].ToString(),
                                    Text = $"{r["Nome"]} {r["Cognome"]}"
                                });
                            }
                            return new SelectList(customers, "Value", "Text");
                        }
                    }
                }
            }

            return null;
        }

        private SelectList GetRoomsDropdown()
        {
            string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                using (SqlCommand c = new SqlCommand(@"SELECT ID, Number, Descrizione FROM Rooms", conn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            List<SelectListItem> rooms = new List<SelectListItem>();
                            while (r.Read())
                            {
                                rooms.Add(new SelectListItem
                                {
                                    Value = r["ID"].ToString(),
                                    Text = $"{r["Number"]} - {r["Descrizione"]}"
                                });
                            }
                            return new SelectList(rooms, "Value", "Text");
                        }
                    }
                }
            }

            return null;
        }

        private Booking GetBookingById(int id)
        {
            string connect = ConfigurationManager.ConnectionStrings["DB"].ToString();
            using (var conn = new SqlConnection(connect))
            {
                conn.Open();
                var c = new SqlCommand(@"
            SELECT 
                b.*, 
                c.Nome, c.Cognome, 
                r.Number, r.Descrizione, r.SingolaDoppia
            FROM Booking b
            INNER JOIN Customers c ON b.IDCliente = c.ID
            INNER JOIN Rooms r ON b.IDStanza = r.ID
            WHERE b.ID = @ID
        ", conn);

                c.Parameters.AddWithValue("@ID", id);

                using (var reader = c.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Booking
                        {
                            ID = (int)reader["ID"],
                            IDCliente = (int)reader["IDCliente"],
                            IDStanza = (int)reader["IDStanza"],
                            DataPrenotazione = (DateTime)reader["DataPrenotazione"],
                            CheckIn = (DateTime)reader["CheckIn"],
                            CheckOut = (DateTime)reader["CheckOut"],
                            Caparra = (decimal)reader["Caparra"],
                            Tariffa = (decimal)reader["Tariffa"],
                            PensioneOmezza = (bool)reader["PensioneOmezza"],
                            Colazione = (bool)reader["Colazione"],
                            CustomerName = (string)reader["Nome"] + " " + (string)reader["Cognome"],
                            RoomNumber = (int)reader["Number"],
                            RoomDescription = (string)reader["Descrizione"],
                            SingolaDoppia = (bool)reader["SingolaDoppia"]
                        };
                    }
                }
            }

            return null;
        }
    }
}
