using System.ComponentModel.DataAnnotations;

namespace Architecture.ViewModel
{
    public class InsertCustomerAsync
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Mail { get; set; }
        
    }
}