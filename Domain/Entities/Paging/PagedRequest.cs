namespace Domain.Entities.Paging
{
    /// <summary>
    /// Base request for paged data
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public class PagedRequest
    {
        private int _pageSize;
        private string? _sort = "created_at DESC";

        /// <summary>
        /// Page index (0-based)
        /// </summary>
        public virtual int PageIndex { get; set; }

        /// <summary>
        /// Size of a page. If pageSize = -1, return all records
        /// </summary>
        public virtual int PageSize 
        { 
            get => _pageSize;
            set => _pageSize = value;
        }

        /// <summary>
        /// Only get active records
        /// </summary>
        public virtual bool? IsActive { get; set; } = true;

        /// <summary>
        /// SQL ORDER BY clause
        /// Example: "created_at DESC"
        /// </summary>
        public virtual string? Sort 
        { 
            get => _sort;
            set => _sort = value;
        }

        /// <summary>
        /// Use stored procedure to get data
        /// </summary>
        public virtual bool? IsUseProc { get; set; } = false;
    }
}
