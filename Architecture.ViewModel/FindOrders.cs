using System;

namespace Architecture.ViewModel
{
    public class FindOrders
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMail { get; set; }

        public int DetailCount { get; set; }
    }
}
