using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using Login = S18_compito.Models.Login;

namespace S18_compito.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index (Login login) 
        {
            string connection  = ConfigurationManager.ConnectionStrings["DB"].ToString();
            var conn = new SqlConnection(connection);
            conn.Open();
            var c = new SqlCommand(@"
               SELECT *.

               FROM Admin
               WHERE Username = @username AND Password = @password
               ", conn);
            c.Parameters.AddWithValue("@username", login.Username);
            c.Parameters.AddWithValue("@password", login.Password);

            var r = c.ExecuteReader();

            if (r.HasRows)
            {
                r.Read();
                FormsAuthentication.SetAuthCookie(r["ID"].ToString(), false);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}