using MyRestaurantProject.Models.Enums;

namespace MyRestaurantProject.Models
{
    //[FromQuery] string searchPhrase, [FromQuery] int pageNumber, [FromQuery] int pageSize
    public class RestaurantQuery
    {
        public string SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public SortDirection? SortDirection { get; set; }
        public string SortBy { get; set; }
    }
}