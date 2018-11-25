//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DirectoryService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompanyStaff
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
        public int CreatorUID { get; set; }
        public string CreatorIP { get; set; }
        public string CreatorRole { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<int> UpdatorUID { get; set; }
        public string UpdatorIP { get; set; }
        public string UpdatorRole { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual Company Company { get; set; }
    }
}
