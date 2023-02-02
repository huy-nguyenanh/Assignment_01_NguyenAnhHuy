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

namespace eStoreClient.Pages.OrderPage
{
    public class EditModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;

        public EditModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public List<OrderDetail> OrderDetail { get; set; }
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
                        OrderDetail = Order.OrderDetails.ToList();
                        ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Email");
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
                throw new Exception(ex.Message);
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
                    Order.OrderDetails = OrderDetail;
                    var json = JsonConvert.SerializeObject(Order);
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    request.RequestUri = new Uri(BASE_URL + "/Order/update");
                    request.Method = HttpMethod.Put;
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
