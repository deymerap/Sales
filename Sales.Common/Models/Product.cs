﻿namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public String Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime PublishOn { get; set; }
    }
}

