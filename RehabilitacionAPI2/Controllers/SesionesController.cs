using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RehabilitacionAPI2.Models;

namespace RehabilitacionAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionesController : ControllerBase
    {
        // La conexión se crea una sola vez para toda la API.
        // Antes se creaba un MongoClient nuevo en cada petición, lo cual es lento y gasta recursos.
        private static readonly object _candado = new object();
        private static IMongoCollection<Sesion>? _coleccion;

        private readonly IMongoCollection<Sesion> _sesionesCollection;

        public SesionesController(IConfiguration configuracion)
        {
            if (_coleccion == null)
            {
                lock (_candado)
                {
                    if (_coleccion == null)
                    {
                        // La cadena de conexión ya no está escrita en el código.
                        // Se guarda fuera del repositorio con:
                        //   dotnet user-secrets set "MongoDB:ConnectionString" "mongodb+srv://..."
                        var connectionString = configuracion["MongoDB:ConnectionString"]
                            ?? throw new InvalidOperationException(
                                "Falta MongoDB:ConnectionString en la configuración (dotnet user-secrets).");

                        var client = new MongoClient(connectionString);
                        var database = client.GetDatabase("RehabVR_DB");
                        _coleccion = database.GetCollection<Sesion>("sesiones");
                    }
                }
            }

            _sesionesCollection = _coleccion!;
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
        // [ApiController] valida los rangos de Sesion (puntaje 0-100, etc.) y responde 400 si no se cumplen.
        [HttpPost]
        public async Task<ActionResult<Sesion>> Post(Sesion nuevaSesion)
        {
            try
            {
                // La fecha la pone el servidor, así el reloj de quien envíe no afecta los reportes.
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
