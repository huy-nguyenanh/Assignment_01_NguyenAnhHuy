using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace eStoreClient.Pages.OrderPage
{
    public class DeleteModel : PageModel
    {
        public DeleteModel()
        {

        }

        [BindProperty]
        public Order Order { get; set; }

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
                    request.RequestUri = new Uri(BASE_URL + "/Order/" + id);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("ContentType", "application/json");
                    HttpResponseMessage response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        Order = JsonConvert.DeserializeObject<Order>(responseString);
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
                    request.RequestUri = new Uri(BASE_URL + "/Order/delete/" + id);
                    request.Method = HttpMethod.Delete;
                    //request.Properties.Add("product", Product);
                    HttpResponseMessage response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("Order", "CallAPI");
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
