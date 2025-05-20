using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Paging
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public PagedResponse()
        {
            
        }
        public PagedResponse(List<T> items, int totalCount, int pageSize, int pageIndex)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}
