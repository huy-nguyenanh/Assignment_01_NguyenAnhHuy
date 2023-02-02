using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;

namespace eStoreClient.Pages.OrderPage
{
    public class OrderDetailModel : PageModel
    {
        private readonly DataLayer.Models.FStoreDBContext _context;

        public OrderDetailModel(DataLayer.Models.FStoreDBContext context)
        {
            _context = context;
        }

        public IList<OrderDetail> OrderDetail { get; set; }

        public async Task OnGetAsync(int orderId)
        {
            OrderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product).Where(w => w.OrderId == orderId).ToListAsync();
        }
    }
}
