﻿using System.ComponentModel.DataAnnotations;

namespace Store.API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(0.1, int.MaxValue, ErrorMessage = "Quantity must be at least one item")]
        public int Quantity { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price must be more than zero")]
        public decimal Price { get; set; }
    }
}