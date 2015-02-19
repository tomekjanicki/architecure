using System;

namespace Architecture.ViewModel
{
    public class FindProducts
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public byte[] Version { get; set; }
    }
}