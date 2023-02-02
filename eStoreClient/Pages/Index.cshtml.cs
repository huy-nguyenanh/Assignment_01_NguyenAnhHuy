using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eStoreClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        const string BASE_URL = "https://localhost:44351/api";

        [BindProperty]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public void OnGet()
        {
            HttpContext.Session.Clear();
        }
        public string Message { get; set; } = "Request Initiation Waiting...";
        public async Task<IActionResult> OnPostCallAPI()
        {
            string adminEmail = _configuration.GetSection("AdminAccount:email").Value;
            string adminPassword = _configuration.GetSection("AdminAccount:password").Value;
            var role = "";
            if (Email.Equals(adminEmail) && Password.Equals(adminPassword))
            {
                role = "admin";
                HttpContext.Session.SetString("ROLE", role);
                HttpContext.Session.SetString("EMAIL", Email);
                HttpContext.Session.SetString("PASSWORD", Password);
                return RedirectToPage("./ProductPage/Product", "CallAPI");
            }
            else
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        HttpRequestMessage request = new HttpRequestMessage();
                        request.RequestUri = new Uri(BASE_URL + "/Member/login/" + Email + "/" + Password);
                        request.Method = HttpMethod.Get;

                        HttpResponseMessage response = await client.SendAsync(request);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var statusCode = response.StatusCode;
                        if (response.IsSuccessStatusCode)
                        {
                            role = "member";
                            HttpContext.Session.SetString("ROLE", role);
                            HttpContext.Session.SetString("EMAIL", Email);
                            HttpContext.Session.SetString("PASSWORD", Password);
                            return RedirectToPage("./ProductPage/Product", "CallAPI");
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Wrong email or password";
                            return Page();
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }
    }
}
