using AutoMapper;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.DTOs;
using IntivePatronageLibraryCORE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IntivePatronageLibraryAPI.Controllers
{
    //using async because of I/O bound app style

    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookService bookService, IMapper iMapper)
        {
            _bookService = bookService;
            _mapper= iMapper;
        }

        // Read all books – GET: api/books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks([FromQuery]bool withAuthors)
        {
            if (withAuthors)
            {
                var books = await _bookService.GetAllWithAuthors();
                var booksDto = _mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
                return Ok(booksDto);
            }
            else
            {
                var books = await _bookService.GetAll();
                var booksDto = _mapper.Map<IEnumerable<Book>, List<BookDTO>>(books);
                return Ok(booksDto);
            }
        }

        // Update a book – PUT: api/books/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, BookDTO bookDto)
        {
            if (id != bookDto.Id)
            {
                if (bookDto.Id == 0)
                    bookDto.Id = id;
                else
                {
                    ModelState.AddModelError("", "Id cannot be changed!");
                    return BadRequest(ModelState);
                }
            }

            var bookToUpdate = await _bookService.GetBookById(id);

            if (bookToUpdate == null)
                return BadRequest();

            var book = _mapper.Map<Book>(bookDto);
            await _bookService.UpdateBook(bookToUpdate, book);

            return NoContent();
        }

        // Create a book – POST: api/books
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO bookDto)
        {
            if (bookDto.Id != 0)
            {
                ModelState.AddModelError("", "Id must not be included (or with value 0)");
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);
            book = await _bookService.AddBook(book);
            bookDto.Id=book.Id;

            return CreatedAtAction("GetBook", new { id = bookDto.Id }, bookDto);
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

            var book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBook(book);

            return NoContent();
        }

        // // Search book – GET: api/books/search?title=sometitle&firstname=somename
        // [HttpGet("search")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public async Task<ActionResult<Book>> GetBooks([FromQuery] BookQueryObject bookQueryObject)
        // {
        //     var result = await bookQueryObject.SearchBookAuthorAsync();
        //     if (!result.Any())
        //     {
        //         return NotFound();
        //     }
        //     return Ok(result);
        // }

        // Not required endpoints ---

        // GET: api/books/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Id must be greater than 0");
                return BadRequest(ModelState);
            }

            var book = await _bookService.GetWithAuthorsById(id);

            if (book == null)
                return NotFound();

            var bookDto = _mapper.Map<Book>(book);

            return Ok(bookDto);
        }
    }
}