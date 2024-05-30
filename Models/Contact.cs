﻿using System.ComponentModel.DataAnnotations;

namespace Remood_Place.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Descripthon { get; set; }

    }
}
