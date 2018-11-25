using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirectoryService.Dtos
{
    public class CustomerPhoneDto
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}