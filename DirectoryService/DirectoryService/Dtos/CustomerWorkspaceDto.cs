using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirectoryService.Dtos
{
    public class CustomerWorkspaceDto
    {
        [Key]
        public int ID { get; set; }
        //[Required]
        //public int CompanyID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Status { get; set; }
    }
}