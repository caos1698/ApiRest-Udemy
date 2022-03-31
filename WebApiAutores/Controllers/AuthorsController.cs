using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IService service;
        private readonly TransientService transientService;
        private readonly ScopedService scopedService;
        private readonly SingletonService singletonService;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(ApplicationDbContext context, IService service,
            TransientService transientService, ScopedService scopedService,
            SingletonService singletonService, ILogger<AuthorsController> logger )
        {
            this.context = context;
            this.service = service;
            this.transientService = transientService;
            this.scopedService = scopedService;
            this.singletonService = singletonService;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration =)]
        public ActionResult ObtainGuids()
        {
            return Ok(new
            {
                AuthorsControllerTransient = transientService.Guid,
                ServiceA_Transient = service.ObtainTransient(),
                AuthorsControllerScoped = scopedService.Guid,
                ServiceA_Scoped = service.ObtainScoped(),
                AuthorsControllerSingleton = singletonService.Guid,
                ServiceA_Singleton = service.ObtainSingleton(),          
            });
        }

        [HttpGet]
        public async Task<List<Author>> Get()
        {
            logger.LogInformation("We are gettin the authors");
            service.DoTask();
            return await context.Authors.Include(x => x.Books).ToListAsync();
        }

        //Added another get to obtain something different to the original get. Precisely, the first author of them
        [HttpGet("first")]
        public async Task<ActionResult<Author>> FirstAuthor()
        {
            return await context.Authors.FirstOrDefaultAsync();
        }

        //Added another get to obtain something different to the original get. Precisely, the author by its id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x=> x.Id == id);
            if (author == null)
            {
                return NotFound("We couldn´t find the author with this id");
            }

            return author;
        }

        //Added another get to obtain something different to the original get. Precisely, the author by its name
        [HttpGet("{name}")]
        public async Task<ActionResult<Author>> Get(string name)
        {
            var author = await context.Authors.FirstOrDefaultAsync(x => x.Nombre.Contains(name));
            if (author == null)
            {
                return NotFound("We couldn´t find the author with this name");
            }

            return author;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
            context.Add(author);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Author author, int id)
        {
            if (author.Id != id)
            {
                return BadRequest("The Author´s Id does not match the Id given by URL ");
            }

            var existe = await context.Authors.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(author);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Authors.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Author { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
