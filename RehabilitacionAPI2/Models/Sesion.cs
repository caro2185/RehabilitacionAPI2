using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RehabilitacionAPI2.Models
{
    public class Sesion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("idPaciente")]
        //[JsonPropertyName("idPaciente")]
        public string IdPaciente { get; set; } = "";

        [BsonElement("idTerapeuta")]
        //[JsonPropertyName("idTerapeuta")]
        public string IdTerapeuta { get; set; } = "";

        [BsonElement("idTerapia")]
        public int IdTerapia { get; set; }

        [BsonElement("nombreTerapia")]
        public string NombreTerapia { get; set; } = "";

        [BsonElement("puntaje")]
        public int Puntaje { get; set; }

        [BsonElement("precision")]
        public double Precision { get; set; }

        [BsonElement("tiempoSegundos")]
        public int TiempoSegundos { get; set; }

        [BsonElement("fecha")]
        public string Fecha { get; set; } = "";
    }
}
