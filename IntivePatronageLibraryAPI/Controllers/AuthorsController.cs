using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.DTOs;
using IntivePatronageLibraryCORE.Models.QueryObjects;
using IntivePatronageLibraryCORE.Services;
using Newtonsoft.Json;


namespace IntivePatronageLibraryAPI.Controllers
{
    //using async because of I/O bound app style

    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorService authorService, IMapper iMapper)
        {
            _authorService = authorService;
            _mapper = iMapper;
        }

        // Read all authors – GET: api/authors
        // Search, sorting, paging included
        // for example: api/authors?orderBy=lastname desc, firstname&pageNumber=1&pageSize=3&firstname=Jan
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<AuthorDTO>>> GetAuthors([FromQuery] bool withBooks,
            [FromQuery] AuthorQueryParameters authorParams)
        {
            PagedList<Author> authors;
            if (withBooks)
                authors = await _authorService.GetAllWithBooks(authorParams);
            else
                authors = await _authorService.GetAll(authorParams);

            var metadata = GetMetadata(authors);
            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));
            var authorsDto = _mapper.Map<IEnumerable<Author>, List<AuthorDTO>>(authors);
            return Ok(authorsDto);
        }

        // Create an authorDto – POST:  api/authors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDto)
        {
            if (authorDto.Id != 0)
                return BadRequest();

            var author = _mapper.Map<Author>(authorDto);
            author = await _authorService.AddAuthor(author);
            authorDto.Id = author.Id;

            return CreatedAtAction("GetAuthor", new { id = authorDto.Id }, authorDto);
        }

        // Delete an authorDto – DELETE:  api/authors/3
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id <= 0)
                return BadRequest();

            var author = await _authorService.GetAuthorById(id);

            if (author == null)
                return NotFound();

            await _authorService.DeleteAuthor(author);

            return NoContent();
        }

        // Not required endpoints ---

        // read authorDto – GET:  api/authors/3
        // Are we interested in attaching books? I will attach in single get
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDTO>> GetAuthor([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest();

            var author = await _authorService.GetWithBooksById(id);

            if (author == null)
                return NotFound();

            var authorDto = _mapper.Map<AuthorDTO>(author);

            return Ok(authorDto);
        }

        // Assign book to authorDto - POST: api/authors/3/books
        [HttpPost("{id:int}/books")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AuthorAssignBook(int id, [FromBody] int bookId)
        {
            if (id <= 0 || bookId <= 0)
                return BadRequest();

            var author = await _authorService.AddBookToAuthor(id, bookId);

            return CreatedAtAction("GetAuthor", new { id }, author);
        }

        private static object GetMetadata(PagedList<Author> pagelist)
        {
            return new
            {
                pagelist.TotalCount,
                pagelist.PageSize,
                pagelist.CurrentPage,
                pagelist.TotalPages,
                pagelist.HasNext,
                pagelist.HasPrevious
            };
        }
    }
}