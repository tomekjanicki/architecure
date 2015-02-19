using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Architecture.ViewModel
{
    public class UpdateOrder : IValidatableObject
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public byte[] Version { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }


        public class OrderDetail
        {
            public int Id { get; set; }

            [Range(1, int.MaxValue)]
            public int ProductId { get; set; }

            [Range(1, int.MaxValue)]
            public int Quantity { get; set; }

        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!OrderDetails.Any())
                yield return new ValidationResult("At least one position is required");
        }

    }
}