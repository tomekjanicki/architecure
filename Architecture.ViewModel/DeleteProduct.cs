using System.ComponentModel.DataAnnotations;

namespace Architecture.ViewModel
{
    public class DeleteProduct
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public byte[] Version { get; set; }

    }
}