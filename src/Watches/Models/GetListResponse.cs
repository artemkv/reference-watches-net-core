using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.Models
{
    public class GetListResponse<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int Count { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}
