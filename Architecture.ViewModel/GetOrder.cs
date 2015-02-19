using System;
using System.Collections.Generic;

namespace Architecture.ViewModel
{
    public class GetOrder
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMail { get; set; }

        public byte[] Version { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }


        public class OrderDetail
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            public string ProductCode { get; set; }

            public string ProductName { get; set; }

            public decimal ProductPrice { get; set; }

            public int Quantity { get; set; }

            public decimal Total
            {
                get { return Quantity * ProductPrice; }
            }

 
        }
    }
}