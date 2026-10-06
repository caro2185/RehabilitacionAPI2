using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RehabilitacionAPI2.Models;
using Microsoft.Extensions.Configuration;

namespace RehabilitacionAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionesController : ControllerBase
    {
        private readonly IMongoCollection<Sesion> _sesionesCollection;

        public SesionesController(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("RehabVR_DB");
            _sesionesCollection = database.GetCollection<Sesion>("sesiones");
        }

        // GET: api/Sesiones
        [HttpGet]
        public async Task<ActionResult<List<Sesion>>> Get()
        {
            var sesiones = await _sesionesCollection.Find(_ => true).ToListAsync();
            return Ok(sesiones);
        }

        // GET: api/Sesiones/paciente/{idPaciente}
        [HttpGet("paciente/{idPaciente}")]
        public async Task<ActionResult<List<Sesion>>> GetByPaciente(string idPaciente)
        {
            var sesiones = await _sesionesCollection.Find(s => s.IdPaciente == idPaciente).ToListAsync();
            return Ok(sesiones);
        }

        // POST: api/Sesiones
        [HttpPost]
        public async Task<ActionResult<Sesion>> Post(Sesion nuevaSesion)
        {
            try
            {
                nuevaSesion.Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await _sesionesCollection.InsertOneAsync(nuevaSesion);
                return Ok(new { mensaje = " Sesión guardada correctamente", sesion = nuevaSesion });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = " Error al guardar sesión", detalle = ex.Message });
            }
        }
    }
}