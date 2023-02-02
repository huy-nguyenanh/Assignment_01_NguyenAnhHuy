using BussinessLayer.DAOs;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Repos
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderDetails();
        IEnumerable<OrderDetail> GetOrderDetailById(int id);
        void AddOrderDetail(OrderDetail orderDetail);
        void UpdateOrderDetail(IEnumerable<OrderDetail> orderDetail);
        //void RemoveOrderDetail(OrderDetail orderDetail);
    }

    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.Instance.AddOrderDetail(orderDetail);

        public IEnumerable<OrderDetail> GetOrderDetailById(int id) => OrderDetailDAO.Instance.GetOrderDetailByID(id);

        public IEnumerable<OrderDetail> GetOrderDetails() => OrderDetailDAO.Instance.GetOrderDetailList();

        //public void RemoveOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.Instance.RemoveOrderDetail(orderDetail);

        public void UpdateOrderDetail(IEnumerable<OrderDetail> orderDetail) => OrderDetailDAO.Instance.UpdateOrderDetail(orderDetail);
    }
}
