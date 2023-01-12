using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntivePatronageLibraryAPI.Data;
using IntivePatronageLibraryAPI.Models;

namespace IntivePatronageLibraryAPI.Controllers
{
    //using async because of I/O bound app style

    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public BooksController(LibraryDbContext db)
        {
            _db = db;
        }

        // Read all books – GET: api/books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery]bool withAuthors)
        {
            return withAuthors? Ok(await _db.Books.Include(x=>x.Authors).ToListAsync()) : Ok(await _db.Books.ToListAsync());
        }

        // Update a book – PUT: api/books/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                if (book.Id == 0)
                    book.Id = id;
                else
                {
                    ModelState.AddModelError("", "Id cannot be changed!");
                    return BadRequest(ModelState);
                }
            }

            _db.Entry(book).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // book does not exists
                if (!(_db.Books?.Any(e => e.Id == id)).GetValueOrDefault())
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // Create a book – POST: api/books
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (book.Id != 0)
            {
                ModelState.AddModelError("", "Id must not be included (or with value 0)");
                return BadRequest(ModelState);
            }

            // I assume we accept duplicate books

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // Delete a book – DELETE: api/books/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Id must be greater than 0");
                return BadRequest(ModelState);
            }

            var book = await _db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // Search book – GET: api/books/search?title=sometitle&firstname=somename
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBooks([FromQuery] BookQueryObject bookQueryObject)
        {
            var result = await bookQueryObject.SearchBookAuthorAsync();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        // Not required endpoints ---

        // GET: api/books/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Id must be greater than 0");
                return BadRequest(ModelState);
            }

            var book = await _db.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
    }
}