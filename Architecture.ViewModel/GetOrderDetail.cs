namespace Architecture.ViewModel
{
    public class GetOrderDetail
    {
        public int Id { get; set; }

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