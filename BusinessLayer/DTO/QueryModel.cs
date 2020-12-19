using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.DTO
{
    public class QueryModel
    {
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
