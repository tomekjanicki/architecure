using System.ComponentModel.DataAnnotations;

namespace Architecture.ViewModel
{
    public class InsertCustomer
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(Const.MailRegularExpression)]
        public string Mail { get; set; }
        
    }
}