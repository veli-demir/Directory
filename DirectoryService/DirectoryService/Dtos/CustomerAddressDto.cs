using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirectoryService.Dtos
{
    public class CustomerAddressDto
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string Address { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}