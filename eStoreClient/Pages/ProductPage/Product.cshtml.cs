using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using DataLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace eStoreClient.Pages.ProductPage
{
    public class ProductModel : PageModel
    {
        public ProductModel()
        {
        }

        public IList<Product> Product { get; set; }
        public string Role { get; set; }

        const string BASE_URL = "https://localhost:44351/api";

        public string Message { get; set; } = "Request Initiation Waiting...";
        public async Task<IActionResult> OnGetCallAPI()
        {
            Role = HttpContext.Session.GetString("ROLE");
            try
            {
                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(BASE_URL + "/Product/list/");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("ContentType", "application/json");
                    HttpResponseMessage response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        Product = JsonConvert.DeserializeObject<IList<Product>>(responseString);
                        return Page();
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Something went wrong!!! Please try again";
                        return Page();
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Something went wrong!!! Please try again";
                throw new Exception(ex.Message);
            }
        }
    }
}
