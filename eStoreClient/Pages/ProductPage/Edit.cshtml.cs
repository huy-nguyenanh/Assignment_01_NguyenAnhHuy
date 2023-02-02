using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace eStoreClient.Pages.ProductPage
{
    public class EditModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;
        public EditModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        const string BASE_URL = "https://localhost:44351/api";

        public string Message { get; set; } = "Request Initiation Waiting...";
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
                        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                        return Page();
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Not Found Product";
                        return Page();
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Something went wrong";
                string error = ex.Message;
                return Page();
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(Product);
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    request.RequestUri = new Uri(BASE_URL + "/Product/update");
                    request.Method = HttpMethod.Put;
                    //request.Properties.Add("product", Product);
                    HttpResponseMessage response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("Product", "CallAPI");
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Not Found Product";
                        return Page();
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Something went wrong";
                string error = ex.Message;
                return Page();
            }
        }

    }
}

