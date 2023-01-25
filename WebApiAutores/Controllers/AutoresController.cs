using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")] //RUTA == api/autores
    public class AutoresController : ControllerBase //HERENCIA de una clase base, es la tipica para web api
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio,
            ServicioTransient servicioTransient, ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }

        public ApplicationDbContext Context { get; }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get() //ACCIONES(ENDPONIT)metodo/funiocn
        {
            logger.LogInformation("Estamos obteniendo los autores");
            logger.LogWarning("Mensaje de prueba");
            servicio.RealizarTarea();
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

         [HttpGet("primero")] ///api/autores/primero?nombre=agustina'
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
        {
            return await context.Autores.FirstOrDefaultAsync();  //Obteniendo el primer registro de la tabla, o nulo si no hay registro

        }

        //Aca no se necesita Async... 
        [HttpGet("primero2")]
        public ActionResult<Autor> PrimerAutor2()
        {
            return new Autor() { Nombre = "inventado" };

        }

        [HttpGet("{id:int}/{param2?}")] // ? me perimite poder mandar una variable vacia,
                                          // o se le puede dar un valor por defecto siendo{param2=persona}
        public async Task<ActionResult<Autor>> Get(int id, string param2)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            //FirstOrDefaultAsync == Retorna el primer registro q tenga la caracteristica
            if (autor == null)
            {
                return NotFound();  //devuelve un 404
            }
            return autor;
        }

        //Buscar un autor por su nombre
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get([FromRoute] string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }


        [HttpGet("GUID")]
       
        public ActionResult ObtenerGuids()
        {
            return Ok(new{
                AutoresController_Transient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                AutoresController_Scoped = servicioScoped.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),
                AutoresController_Singleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.ObtenerSingleton()
            }); 
        }

    
        [HttpPost] //LA RUTA ES: api/autores
        [HttpPost("agregar")] //LA RUTA ES: api/autores/agregar
        [HttpPost("/agregar")]//LA RUTA ES: agregar
        public async Task<ActionResult> Post([FromBody] Autor autor)
        {
            var existeAutorConMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if (existeAutorConMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");
            }
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id:int}")] //api/autores/n°
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
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
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id }); //Aca se marca el autor que sera eliminado
            await context.SaveChangesAsync(); //Aca se realizan los cambios 
            return Ok();

        }
    }
}
