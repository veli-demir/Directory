using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectoryPortal.Dtos
{
    public class AuthorizedDto
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}