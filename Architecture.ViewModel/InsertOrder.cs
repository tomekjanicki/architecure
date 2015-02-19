using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Architecture.ViewModel
{
    public class InsertOrder : IValidatableObject
    {
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public class OrderDetail
        {
            [Range(1, int.MaxValue)]
            public int ProductId { get; set; }

            [Range(1, int.MaxValue)]
            public int Quantity { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderDetails == null || !OrderDetails.Any())
                yield return new ValidationResult("At least one position is required");
        }


    }
}