using AutoMapper;
using BussinessLayer.Repos;
using DataLayer.Models;
using DataLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace eStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        [HttpGet("list")]
        public IActionResult Gets()
        {
            var orders = _orderRepo.GetOrders();
            if (orders == null || orders.Count() == 0)
            {
                return BadRequest("Empty Data");
            }
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _orderRepo.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPost("create")]
        public IActionResult Post([FromBody] OrderViewModel order)
        {
            try
            {
                var createOrder = _mapper.Map<Order>(order);
                _orderRepo.AddOrder(createOrder);
                return Created("", createOrder);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("update")]
        public IActionResult Put([FromBody] OrderViewModel order)
        {
            try
            {
                var updateOrder = _orderRepo.GetOrderById(order.OrderId);
                if (updateOrder == null)
                {
                    return NotFound();
                }
                _mapper.Map<OrderViewModel, Order>(order, updateOrder);
                _orderRepo.UpdateOrder(updateOrder);
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var deleteOrder = _orderRepo.GetOrderById(id);
            if (deleteOrder == null)
            {
                return NotFound();
            }
            _orderRepo.RemoveOrder(deleteOrder);
            return Ok();
        }
        [HttpGet("report")]
        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var report = _orderRepo.Report(startDate, endDate);
                return Ok(report);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
