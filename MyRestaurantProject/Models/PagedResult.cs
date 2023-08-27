using System;
using System.Collections.Generic;

namespace MyRestaurantProject.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemTo { get; set; }

        public PagedResult(List<T> items, int totalItems, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItems = totalItems;
            ItemsFrom = pageSize * pageNumber;
            ItemTo = ItemsFrom + pageSize;
            TotalPages = (int)Math.Ceiling(TotalItems / (double)pageSize) ;
        }
    }
}