using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectoryService.Dtos
{
    public class ResponseObjectDto
    {
        public ResponseObjectDto() { }

        public ResponseObjectDto(object result, int allItemCount, string exc, int httpStatusCode)
        {
            this.Result = result;
            this.AllItemCount = allItemCount;
            this.Exception = exc;
            this.HttpStatusCode = httpStatusCode;
        }

        public object Result { get; set; }
        public int AllItemCount { get; set; }
        public string Exception { get; set; }
        public int HttpStatusCode { get; set; }
    }
}