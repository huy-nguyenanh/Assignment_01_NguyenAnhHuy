using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataLayer.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace eStoreClient.Pages.ProductPage
{
    public class CreateModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;

        public CreateModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        const string BASE_URL = "https://localhost:44351/api";

        public string Message { get; set; } = "Request Initiation Waiting...";
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
                    request.RequestUri = new Uri(BASE_URL + "/Product/create");
                    request.Method = HttpMethod.Post;
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
                        ViewData["ErrorMessage"] = "Create failed";
                        return Page();
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Something went wrong!";
                string error = ex.Message;
                return Page();
            }
        }
    }
}
