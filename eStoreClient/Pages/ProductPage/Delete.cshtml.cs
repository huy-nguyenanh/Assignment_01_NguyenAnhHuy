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
using System.Text;

namespace eStoreClient.Pages.ProductPage
{
    public class DeleteModel : PageModel
    {
        public DeleteModel()
        {

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
                        return Page();
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Not found product";
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


        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(BASE_URL + "/Product/delete/" + id);
                    request.Method = HttpMethod.Delete;
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
                        ViewData["ErrorMessage"] = "Delete failed";
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
