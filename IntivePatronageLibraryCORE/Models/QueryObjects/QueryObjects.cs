
namespace IntivePatronageLibraryCORE.Models.QueryObjects
{
    public abstract class QueryObject
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


    public class AuthorQueryObject : QueryObject
    {
        public AuthorQueryObject()
        {
            OrderBy = "lastName";
        }

        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public bool? Gender { get; set; } = null;

        // public async Task<IEnumerable<Author>> SearchAuthorAsync()
        // {
        //     var dboptions = new DbContextOptionsBuilder<LibraryDbContext>()
        //         .UseSqlServer(Settings.ConnectionString)
        //         .Options;
        //     await using var db = new LibraryDbContext(dboptions);
        //     var authors = db.Author.AsQueryable();
        //     if (LastName != null)
        //         authors = authors.Where(
        //             x => x.LastName.ToLower() == LastName.ToLower());
        //     if (FirstName != null)
        //         authors = authors.Where(x => x.FirstName.ToLower() == FirstName.ToLower());
        //     if (BirthDate != null)
        //         authors = authors.Where(x => x.BirthDate.Equals(BirthDate));
        //     if (Gender != null)
        //         authors = authors.Where(x => x.Gender == Gender);
        //     return await authors.ToListAsync();
        // }
    }

    public class BookQueryObject : QueryObject
    {
        public BookQueryObject()
        {
            OrderBy = "title";
        }

        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Rating { get; set; } = null;
        public decimal? Rating_from { get; set; } = null;
        public decimal? Rating_to { get; set; } = null;
        public string? ISBN { get; set; } = null;
        public DateTime? PublicationDate { get; set; } = null;

        // public async Task<IEnumerable<Book>> SearchBookAuthorAsync()
        // {
        //     Task<IEnumerable<Author>>? authorsTask = null;
        //     // If any of authors parameters are given, search for matching author
        //     if (FirstName != null || LastName != null || BirthDate != null || Gender != null)
        //     {
        //         authorsTask = SearchAuthorAsync();
        //     }
        //
        //     var booksTask = SearchBookAsync();
        //     var matchingBooks = await booksTask;
        //     // If there are no author values specified retrun all matching books
        //     if (authorsTask == null)
        //         return matchingBooks;
        //     // If there are authors filtered, select books with authors meeting the criteria
        //     var matchingAuthors = (await authorsTask).Select(x => x.Id).ToList();
        //     matchingBooks = matchingBooks.Where(book => book.Authors.Any(author => matchingAuthors.Contains(author.Id) ))
        //         .ToList();
        //     return matchingBooks;
        // }

        // public async Task<IEnumerable<Book>> SearchBookAsync()
        // {
        //     var dboptions = new DbContextOptionsBuilder<LibraryDbContext>()
        //         .UseSqlServer(Settings.ConnectionString)
        //         .Options;
        //     await using var db = new LibraryDbContext(dboptions);
        //     var books = db.Books.Include(x => x.Authors).AsQueryable();
        //     if (Title != null)
        //         books = books.Where(x => x.Title.ToLower() == Title.ToLower());
        //     if (Description != null)
        //         books = books.Where(x => x.Description.ToLower() == Description.ToLower());
        //     if (Rating != null)
        //         books = books.Where(x => x.Rating == Rating);
        //     if (Rating_from != null && Rating_to != null)
        //         books = books.Where(x => x.Rating <= Rating_to && x.Rating >= Rating_from);
        //     else if (Rating_from != null)
        //         books = books.Where(x => x.Rating >= Rating_from);
        //     else if (Rating_to != null)
        //         books = books.Where(x => x.Rating <= Rating_to);
        //     if (ISBN != null)
        //         books = books.Where(x => x.ISBN.ToLower() == ISBN.ToLower());
        //     if (PublicationDate != null)
        //         books = books.Where(x => x.PublicationDate.Equals(PublicationDate));
        //     return await books.ToListAsync();
        // }
    }
}