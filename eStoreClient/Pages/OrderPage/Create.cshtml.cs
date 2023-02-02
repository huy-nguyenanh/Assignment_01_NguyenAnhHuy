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

namespace eStoreClient.Pages.OrderPage
{
    public class CreateModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;


        public CreateModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public OrderDetail OrderDetail { get; set; }
        public IActionResult OnGet(List<OrderDetail> orderDetails)
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Email");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            return Page();
        }
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
                var unitPrice = GetUnitPrice(OrderDetail.ProductId);
                OrderDetail.UnitPrice = unitPrice.Result;
                Order.OrderDetails.Add(OrderDetail);
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(Order);
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                    request.RequestUri = new Uri(BASE_URL + "/Order/create");
                    request.Method = HttpMethod.Post;
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
        private async Task<decimal> GetUnitPrice(int productId)
        {
            decimal unitPrice = 0;
            try
            {
                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(BASE_URL + "/Product/" + productId);
                    request.Method = HttpMethod.Get;
                    HttpResponseMessage response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        var product = JsonConvert.DeserializeObject<Product>(responseString);
                        unitPrice = product.UnitPrice;
                        return unitPrice;
                    }
                    else
                    {
                        return 0;
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
