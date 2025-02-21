using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Scanton.Models; // Update namespace based on your project's structure

namespace Scanton.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Display Login Page
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View(new LoginModel());
        }

        // Process Login Form Submission
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Username", model.Username);
                        command.Parameters.AddWithValue("@Password", model.Password); // Use hashed password comparison in production.

                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        if (count == 1)
                        {
                            HttpContext.Session.SetString("Username", model.Username);
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid username or password");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error for troubleshooting
                    ModelState.AddModelError("", "An error occurred. Please try again later.");
                }
            }

            return View(model);
        }

        // Logout Method
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult ParentCategory()
        {
            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
    }
}
