using System.ComponentModel.DataAnnotations;
using IntivePatronageLibraryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace IntivePatronageLibraryAPI.Models
{
    

    //Searching
    public class AuthorParameters
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public bool? Gender { get; set; } = null;

        public async Task<IEnumerable<Author>> SearchAsync(LibraryDbContext db)
        {
            var authors = await db.Author.ToListAsync();
            if (LastName != null)
                authors = authors.Where(x => string.Equals(x.LastName, LastName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (FirstName != null)
                authors = authors.Where(x => string.Equals(x.FirstName, FirstName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (BirthDate != null)
                authors = authors.Where(x => x.BirthDate.Equals(BirthDate)).ToList();
            if (Gender != null)
                authors = authors.Where(x => x.Gender == Gender).ToList();
            return authors;
        }
    }

    public class BookParamethers
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Rating { get; set; } = null;
        public string? ISBN { get; set; } = null;
        public DateTime? PublicationDate { get; set; } = null;

        public async Task<IEnumerable<Book>> SearchAsync(LibraryDbContext db)
        {
            var books = await db.Books.ToListAsync();
            if (Title != null)
                books = books.Where(x => string.Equals(x.Title, Title, StringComparison.OrdinalIgnoreCase)).ToList();
            if (Description != null)
                books = books.Where(x => string.Equals(x.Description, Description, StringComparison.OrdinalIgnoreCase)).ToList();
            if (Rating != null)
                books = books.Where(x => x.Rating == Rating).ToList();
            if (PublicationDate != null)
                books = books.Where(x => x.PublicationDate.Equals(PublicationDate)).ToList();
            return books;
        }
    }
}