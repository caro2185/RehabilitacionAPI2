using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RehabilitacionAPI2.Models  // O el nombre de tu proyecto
{
    [BsonIgnoreExtraElements]  // 👈 ESTA LÍNEA IGNORA CAMPOS COMO "fecha_registro"

    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string cedula { get; set; } = "";
        public string nombre { get; set; } = "";
        public string email { get; set; } = "";
        public int edad { get; set; }
        public string rol { get; set; } = "";
        public string contraseña { get; set; } = "";

        [BsonRepresentation(BsonType.ObjectId)]
        public string? id_terapeuta { get; set; }  // Solo para pacientes

        public string fecha_registro { get; set; } = "";
        public bool activo { get; set; } = true;


    }
}