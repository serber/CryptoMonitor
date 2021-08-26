using System;

namespace CryptoMonitor.WebApp.Models
{
    public class PaginationModel
    {
        public PaginationModel(int pageSize, long itemsCount)
        {
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);
        }
        
        public int PageSize { get; }

        public int TotalPages { get; }

        public string Action { get; set; }
    }
}