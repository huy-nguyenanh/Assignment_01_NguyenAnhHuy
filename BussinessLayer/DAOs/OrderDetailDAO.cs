using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.DAOs
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }

        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetailList()
        {
            List<OrderDetail> orderDetails;
            try
            {
                var myStoreDB = new FStoreDBContext();
                orderDetails = myStoreDB.OrderDetails.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetails;
        }

        public IEnumerable<OrderDetail> GetOrderDetailByID(int orderDetailID)
        {
            IEnumerable<OrderDetail> orderDetail = null;
            try
            {
                var myStoreDB = new FStoreDBContext();
                //orderDetail = myStoreDB.OrderDetails.SingleOrDefault(orderDetail => orderDetail.OrderId == orderDetailID);
                orderDetail = myStoreDB.OrderDetails.Where(w => w.OrderId == orderDetailID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetail;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                IEnumerable<OrderDetail> p = GetOrderDetailByID((int) orderDetail.OrderId);
                if (p == null || !p.Any())
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.OrderDetails.Add(orderDetail);
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The orderDetail has already existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrderDetail(IEnumerable<OrderDetail> orderDetail)
        {
            try
            {
                IEnumerable<OrderDetail> p = GetOrderDetailByID((int) orderDetail.FirstOrDefault().OrderId);
                if (p != null || p.Any())
                {
                    var myStoreDB = new FStoreDBContext();
                    foreach (var item in orderDetail)
                    {
                        myStoreDB.Entry<OrderDetail>(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The orderDetail has not existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public void RemoveOrderDetail(OrderDetail orderDetail)
        //{
        //    try
        //    {
        //        OrderDetail p = GetOrderDetailByID((int) orderDetail.OrderId);
        //        if (p != null)
        //        {
        //            var myStoreDB = new FStoreDBContext();
        //            myStoreDB.Remove(p);
        //            myStoreDB.SaveChanges();
        //        }
        //        else
        //        {
        //            throw new Exception("The orderDetail has not existed!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
