using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RehabilitacionAPI2.Models;

namespace RehabilitacionAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IMongoCollection<Usuario> _usuariosCollection;

        public UsuariosController()
        {
            var connectionString = "mongodb+srv://karolinajaimes15_db_user:Caro2185_110@cluster0.v9gexas.mongodb.net/?appName=Cluster0";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("RehabVR_DB");
            _usuariosCollection = database.GetCollection<Usuario>("usuarios");
        }

        // ============================================
        // 1. GET: api/Usuarios (todos - solo admin/terapeuta)
        // ============================================
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuarios = await _usuariosCollection.Find(_ => true).ToListAsync();
            return Ok(usuarios);
        }

        // ============================================
        // 2. LOGIN: GET api/Usuarios/login/{cedula}?contrasena=xxx
        // ============================================
        [HttpGet("login/{cedula}")]
        public async Task<ActionResult<Usuario>> Login(string cedula, [FromQuery] string contrasena)
        {
            var usuario = await _usuariosCollection.Find(u => u.cedula == cedula).FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            if (usuario.contraseña != contrasena)
                return Unauthorized(new { mensaje = "Contraseña incorrecta" });

            return Ok(usuario);
        }

        // ============================================
        // 3. REGISTRAR PACIENTE: POST /api/Usuarios/registro
        // (Solo el terapeuta puede hacer esto)
        // ============================================
        [HttpPost("registro")]
        public async Task<ActionResult<Usuario>> RegistrarPaciente(Usuario nuevoPaciente)
        {
            // Verificar que no exista otra cédula igual
            var existe = await _usuariosCollection.Find(u => u.cedula == nuevoPaciente.cedula).FirstOrDefaultAsync();
            if (existe != null)
                return BadRequest(new { mensaje = "Ya existe un usuario con esa cédula" });

            // Asignar campos automáticos
            nuevoPaciente.rol = "paciente";
            nuevoPaciente.fecha_registro = DateTime.Now.ToString("yyyy-MM-dd");
            nuevoPaciente.activo = true;

            await _usuariosCollection.InsertOneAsync(nuevoPaciente);
            return Ok(new { mensaje = "Paciente registrado correctamente", usuario = nuevoPaciente });
        }

        // ============================================
        // 4. GET: api/Usuarios/mis-pacientes/{idTerapeuta}
        // Devuelve SOLO los pacientes de un terapeuta
        // ============================================
        [HttpGet("mis-pacientes/{idTerapeuta}")]
        public async Task<ActionResult<List<Usuario>>> GetMisPacientes(string idTerapeuta)
        {
            var pacientes = await _usuariosCollection
                .Find(u => u.rol == "paciente" && u.id_terapeuta == idTerapeuta)
                .ToListAsync();

            return Ok(pacientes);
        }
    }
}