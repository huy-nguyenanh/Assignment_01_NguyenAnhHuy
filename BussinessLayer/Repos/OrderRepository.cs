using BussinessLayer.DAOs;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Repos
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> SearchOrders(string search);
        Order GetOrderById(int id);
        IEnumerable<Order> GetOrderByMemberEmail(string email);
        Order AddOrder(Order order);
        void UpdateOrder(Order order);
        void RemoveOrder(Order order);
        IEnumerable<Order> Report(DateTime? startDate, DateTime? endDate);
    }

    public class OrderRepository : IOrderRepository
    {
        public Order AddOrder(Order order) => OrderDAO.Instance.AddOrder(order);

        public Order GetOrderById(int id) => OrderDAO.Instance.GetOrderByID(id);
        public IEnumerable<Order> GetOrderByMemberEmail(string email) => OrderDAO.Instance.GetOrderByMemberEmail(email);

        public IEnumerable<Order> GetOrders() => OrderDAO.Instance.GetOrderList();

        public IEnumerable<Order> SearchOrders(string search) => OrderDAO.Instance.SearchOrders(search);

        public void RemoveOrder(Order order) => OrderDAO.Instance.RemoveOrder(order);

        public void UpdateOrder(Order order) => OrderDAO.Instance.UpdateOrder(order);

        public IEnumerable<Order> Report(DateTime? startDate, DateTime? endDate) => OrderDAO.Instance.Report(startDate, endDate);
    }
}
