using System.ComponentModel.DataAnnotations;
using IntivePatronageLibraryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace IntivePatronageLibraryAPI.Models
{
    //Searching
    public class AuthorSearchUnit
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public bool? Gender { get; set; } = null;

        public async Task<IEnumerable<Author>> SearchAuthorAsync(LibraryDbContext db)
        {
            var authors = await db.Author.ToListAsync();
            if (LastName != null)
                authors = authors.Where(x => string.Equals(x.LastName, LastName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            if (FirstName != null)
                authors = authors.Where(x => string.Equals(x.FirstName, FirstName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            if (BirthDate != null)
                authors = authors.Where(x => x.BirthDate.Equals(BirthDate)).ToList();
            if (Gender != null)
                authors = authors.Where(x => x.Gender == Gender).ToList();
            return authors;
        }
    }

    public class BookSearchUnit : AuthorSearchUnit
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Rating { get; set; } = null;
        public string? ISBN { get; set; } = null;
        public DateTime? PublicationDate { get; set; } = null;

        public async Task<IEnumerable<Book>> SearchBookAuthorAsync(LibraryDbContext db)
        {
            IEnumerable<Author>? matchingAuthors = null;
            Task<IEnumerable<Author>>? authorsTask = null;
            if (FirstName != null || LastName != null || BirthDate != null || Gender != null)
            {
                authorsTask = SearchAuthorAsync(db);
            }
            var booksTask = SearchBookAuthorAsync(db);
            var matchingBooks = await booksTask;
            // If there are no author values specified retrun all matching books
            if (authorsTask == null)
                return matchingBooks;
            // If there are authors filtered, select books with authors meeting the criteria
            matchingAuthors = await authorsTask;
            matchingBooks = matchingBooks.Where(book => book.Authors.Any(author => matchingAuthors.Contains(author))).ToList();
            return matchingBooks;
        }

        public async Task<IEnumerable<Book>> SearchBookAsync(LibraryDbContext db)
        {
            var books = await db.Books.Include(x=>x.Authors).ToListAsync();
            if (Title != null)
                books = books.Where(x => string.Equals(x.Title, Title, StringComparison.OrdinalIgnoreCase)).ToList();
            if (Description != null)
                books = books.Where(x => string.Equals(x.Description, Description, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            if (Rating != null)
                books = books.Where(x => x.Rating == Rating).ToList();
            if (ISBN != null)
                books = books.Where(x => string.Equals(x.ISBN, ISBN, StringComparison.OrdinalIgnoreCase)).ToList();
            if (PublicationDate != null)
                books = books.Where(x => x.PublicationDate.Equals(PublicationDate)).ToList();
            return books;
        }
    }
}