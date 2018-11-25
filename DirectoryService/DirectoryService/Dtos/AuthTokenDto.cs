using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectoryService.Dtos
{
    public class AuthTokenDto
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
}