using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace eStoreClient.Pages.ProductPage
{
    public class DetailModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;

        public DetailModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }
        public string Role { get; set; }

        const string BASE_URL = "https://localhost:44351/api";

        public string Message { get; set; } = "Request Initiation Waiting...";
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Role = HttpContext.Session.GetString("ROLE");

            try
            {
                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(BASE_URL + "/Product/" + id);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("ContentType", "application/json");
                    HttpResponseMessage response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        Product = JsonConvert.DeserializeObject<Product>(responseString);
                        return Page();
                    }
                    else
                    {
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
