namespace Domain.Entities.Paging
{
    /// <summary>
    /// Common paged request for all admin operations
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public class BaseAdminPagedRequest
    {
        private const int DEFAULT_PAGE_SIZE = 50;

        /// <summary>
        /// Page index (0-based)
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// Size of a page, can be of set [5, 10, 20, 50, 100]
        /// </summary>
        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

        /// <summary>
        /// Field name to sort by (in camel case or pascal case)
        /// </summary>
        public string? SortField { get; set; }

        /// <summary>
        /// Sort order ("asc" or "desc")
        /// </summary>
        public string? SortOrder { get; set; }

        /// <summary>
        /// Search term
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Fields to search (in camel case or pascal case)
        /// </summary>
        public List<string>? SearchFields { get; set; }
    }
}