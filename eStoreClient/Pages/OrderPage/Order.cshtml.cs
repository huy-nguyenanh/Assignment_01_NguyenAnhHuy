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
using Microsoft.AspNetCore.Http;

namespace eStoreClient.Pages.OrderPage
{
    public class OrderModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;

        public OrderModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; }
        public string Role { get; set; }
        const string BASE_URL = "https://localhost:44351/api";
        public string Message { get; set; } = "Request Initiation Waiting...";
        public async Task<IActionResult> OnGetCallAPI()
        {
            Role = HttpContext.Session.GetString("ROLE");
            if (Role.Equals("admin"))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        HttpRequestMessage request = new HttpRequestMessage();
                        request.RequestUri = new Uri(BASE_URL + "/Order/list/");
                        request.Method = HttpMethod.Get;
                        request.Headers.Add("ContentType", "application/json");
                        HttpResponseMessage response = await client.SendAsync(request);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var statusCode = response.StatusCode;
                        if (response.IsSuccessStatusCode)
                        {
                            Order = JsonConvert.DeserializeObject<IList<Order>>(responseString);
                            return Page();
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Empty data";
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
            else
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var memberEmail = HttpContext.Session.GetString("EMAIL");
                        HttpRequestMessage request = new HttpRequestMessage();
                        request.RequestUri = new Uri(BASE_URL + "/Order/member/" + memberEmail);
                        request.Method = HttpMethod.Get;
                        request.Headers.Add("ContentType", "application/json");
                        HttpResponseMessage response = await client.SendAsync(request);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var statusCode = response.StatusCode;
                        if (response.IsSuccessStatusCode)
                        {
                            Order = JsonConvert.DeserializeObject<IList<Order>>(responseString);
                            return Page();
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Empty data";
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
}
