using S18_compito.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace S18_compito.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Rooms> rooms = GetRooms();
            return View(rooms);
        }

        private List<Rooms> GetRooms()
        {
            string connect= ConfigurationManager.ConnectionStrings["DB"].ToString();
            List<Rooms> rooms = new List<Rooms>();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                using (SqlCommand c = new SqlCommand(@"SELECT * FROM Rooms", conn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                Rooms room = new Rooms();
                                room.Number = (int)r["Number"];
                                room.Descrizione = (string)r["Descrizione"];
                                room.SingolaDoppia = (bool)r["SingolaDoppia"];
                                rooms.Add(room);
                            }
                        }
                    }
                }
            }
            return rooms;
        }
    }
}