namespace Architecture.ViewModel
{
    public class Const
    {
        public class Roles
        {
            public const string Administrator = "Administrator";
            public const string OrderManager = "Order Manager";
            public const string ProductManager = "Product Manager";
            public const string CustomerManager = "Customer Manager";
        }

        public class OrderStatus
        {
            public const string New = "New";
            public const string Confirmed = "Confirmed";
            public const string Closed = "Closed";
        }

        public const string MailRegularExpression = @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})";

    }
}
