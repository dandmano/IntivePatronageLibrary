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

    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public AuthorsController(LibraryDbContext db)
        {
            _db = db;
        }

        // Read all authors – GET: api/authors
        // Are we interested in attaching books? I won't attach in lists
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return Ok(await _db.Author.ToListAsync());
        }

        // Read all authors – GET: api/authors/search
        // Are we interested in attaching books? I won't attach in lists
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsByName([FromQuery] AuthorSearchUnit authorSearchUnit)
        {
            var result = await authorSearchUnit.SearchAuthorAsync(_db);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        // Create an author – POST:  api/authors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (author.Id != 0)
            {
                ModelState.AddModelError("", "Id must not be included (or with value 0)");
                return BadRequest(ModelState);
            }

            //Check for duplicate in database
            var ap = new AuthorSearchUnit
            {
                BirthDate = author.BirthDate,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Gender = author.Gender
            };
            var duplicates = await ap.SearchAuthorAsync(_db);
            if (duplicates.Any())
                return Conflict("Author with given name and birthdate exists in database");

            _db.Author.Add(author);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // Delete an author – DELETE:  api/authors/3
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("","Id must be greater than 0");
                return BadRequest(ModelState);
            }

            var author = await _db.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _db.Author.Remove(author);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // Not required endpoints ---

        // read author – GET:  api/authors/3
        // Are we interested in attaching books? I will attach in single get
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Author>> GetAuthor([FromRoute] int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Id must be greater than 0");
                return BadRequest(ModelState);
            }

            var author = await _db.Author.Where(x => x.Id == id).Include(x => x.Books).FirstOrDefaultAsync();

            if (author == null)
                return NotFound();

            return Ok(author);
        }

        // Assign book to author - POST: api/authors/3/books
        [HttpPost("{id}/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AuthorAssignBook(int id, [FromBody]int bookId)
        {
            if (id <= 0 || bookId <=0)
            {
                ModelState.AddModelError("","Both ids are required to be grater than 0");
                return BadRequest(ModelState);
            }

            var authorTask = _db.Author.FindAsync(id);
            var bookTask = _db.Books.FindAsync(bookId);

            var author = await authorTask;
            var book = await bookTask;
            if ( author == null || book == null)
            {
                return NotFound();
            }

            author.Books.Add(book);

            return CreatedAtAction("GetAuthor", new { id = id }, author);
        }
    }
}