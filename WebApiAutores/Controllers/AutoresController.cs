using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AutoresController(ApplicationDbContext context)
        {
           this.context=context;
        }

        public ApplicationDbContext Context { get; }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id:int}")] //api/autores/n°
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if(autor.Id != id)
            {
                return BadRequest("El ID del autor no conincide con el di del URL");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor); //Aca se marca el autor que sera actualizado
            await context.SaveChangesAsync();//Aca se realizan los cambios
            return Ok();
        }

        [HttpDelete("{id:int}")] //api/autores/n°
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            if(!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id }); //Aca se marca el autor que sera eliminado
            await context.SaveChangesAsync(); //Aca se realizan los cambios 
            return Ok();

        }
    }
}
