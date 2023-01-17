using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")] //RUTA == api/autores
    public class AutoresController: ControllerBase //HERENCIA de una clase base, es la tipica para web api
    {
        private readonly ApplicationDbContext context;
        public AutoresController(ApplicationDbContext context)
        {
           this.context=context;
        }

        public ApplicationDbContext Context { get; }

        //ACCION== Metodo que se encuentra dentro del controlador
        //          el cual se va a ejecutar en respuesta a un pedido Http realizada
        //          a la ruta definida por el controlador (get/post)

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get() //ACCIONES(ENDPONIT)metodo/funiocn
        {
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        //async por que vamos a devolver info de una base de datos,es buena practica
        //
        [HttpGet("primero")] //aca la ruta va a concatenar con primero api/autores/primero, sino tengo dos pedidos get con la misma ruta
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync(); 
            //Obteniendo el primer registro de la tabla o nulo si no hay registro
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
