namespace IntivePatronageLibraryCORE.Models.QueryObjects
{
    public abstract class QueryParameters
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string OrderBy { get; set; }
    }


    public class AuthorQueryParameters : QueryParameters
    {
        public AuthorQueryParameters()
        {
            OrderBy = "id";
        }

        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public bool? Gender { get; set; } = null;
    }

    public class BookQueryParameters : QueryParameters
    {
        public BookQueryParameters()
        {
            OrderBy = "id";
        }

        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Rating { get; set; } = null;
        public decimal? Rating_from { get; set; } = null;
        public decimal? Rating_to { get; set; } = null;
        public string? ISBN { get; set; } = null;
        public DateTime? PublicationDate { get; set; } = null;
    }
}