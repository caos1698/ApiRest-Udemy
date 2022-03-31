using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            return await context.Books.Include(x=> x.Author).FirstOrDefaultAsync(x => x.Id == id);

        }
        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            var authorExists = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

            if (!authorExists)
            {
                return BadRequest($"The author of id :{book.AuthorId} doesn´t exits");

            }
            context.Add(book);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
