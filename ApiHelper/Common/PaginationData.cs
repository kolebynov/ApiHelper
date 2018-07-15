using System;

namespace RestApi.Common
{
    public class PaginationData
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages => ItemsPerPage > 0 ? (int)Math.Ceiling((double)TotalItems / ItemsPerPage) : 1;
    }
}
