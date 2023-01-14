using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.DTOs;
using IntivePatronageLibraryCORE.Services;


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
            _authorService= authorService;
            _mapper= iMapper;
        }

        // Read all authors – GET: api/authors
        // Are we interested in attaching books? I won't attach in lists
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors([FromQuery]bool withBooks)
        {
            if (withBooks)
            {
                var authors = await _authorService.GetAllWithBooks();
                var authorsDto = _mapper.Map<IEnumerable<Author>, List<AuthorDTO>>(authors);
                return Ok(authorsDto);
            }
            else
            {
                var authors = await _authorService.GetAll();
                var authorsDto = _mapper.Map<IEnumerable<Author>, List<AuthorDTO>>(authors);
                return Ok(authorsDto);
            }
        }

        // Read all authors – GET: api/authors/search
        // Are we interested in attaching books? I won't attach in lists
        // [HttpGet("search")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsByName([FromQuery] AuthorQueryObject authorQueryObject)
        // {
        //     var result = await authorQueryObject.SearchAuthorAsync();
        //     if (!result.Any())
        //     {
        //         return NotFound();
        //     }
        //     return Ok(result);
        // }

        // Create an authorDto – POST:  api/authors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDto)
        {
            if (authorDto.Id != 0)
            {
                ModelState.AddModelError("", "Id must not be included (or with value 0)");
                return BadRequest(ModelState);
            }

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
            {
                ModelState.AddModelError("","Id must be greater than 0");
                return BadRequest(ModelState);
            }

            var author = await _authorService.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }

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
            {
                ModelState.AddModelError("", "Id must be greater than 0");
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> AuthorAssignBook(int id, [FromBody]int bookId)
        {
            if (id <= 0 || bookId <=0)
            {
                ModelState.AddModelError("","Both ids are required to be grater than 0");
                return BadRequest(ModelState);
            }

            var author = await _authorService.AddBookToAuthor(id, bookId);

            return CreatedAtAction("GetAuthor", new { id }, author);
        }
    }
}