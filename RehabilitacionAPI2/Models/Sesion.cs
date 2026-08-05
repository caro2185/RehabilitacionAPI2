using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RehabilitacionAPI2.Models
{
    public class Sesion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("id_paciente")]
        public string IdPaciente { get; set; } = "";

        [BsonElement("id_terapeuta")]
        public string IdTerapeuta { get; set; } = "";

        [BsonElement("id_terapia")]
        public int IdTerapia { get; set; }

        [BsonElement("nombre_terapia")]
        public string NombreTerapia { get; set; } = "";

        [BsonElement("puntaje")]
        public int Puntaje { get; set; }

        [BsonElement("precision")]
        public double Precision { get; set; }

        [BsonElement("tiempo_segundos")]
        public int TiempoSegundos { get; set; }

        [BsonElement("fecha")]
        public string Fecha { get; set; } = "";
    }
}
