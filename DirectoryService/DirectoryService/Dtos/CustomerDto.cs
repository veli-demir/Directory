using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirectoryService.Dtos
{
    public class CustomerDto
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CompanyID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }

        public virtual ICollection<CustomerWorkspaceDto> CR_Customer_CustomerWorkspace { get; set; }
        public virtual ICollection<CustomerAddressDto> CustomerAddress { get; set; }
        public virtual ICollection<CustomerPhoneDto> CustomerPhone { get; set; }
    }
}