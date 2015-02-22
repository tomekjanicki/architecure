﻿using System.ComponentModel.DataAnnotations;

namespace Architecture.ViewModel
{
    public class InsertCustomer
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Mail { get; set; }
        
    }
}