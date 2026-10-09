using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RehabilitacionAPI2.Models
{
    // BsonIgnoreExtraElements: si en la base hay campos que este modelo no conoce
    // (por ejemplo, los que agregue la escena de VR más adelante), la API los ignora
    // en lugar de fallar al leer.
    [BsonIgnoreExtraElements]
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

        // Rendimiento global, de 0 a 100.
        [BsonElement("puntaje")]
        [Range(0, 100)]
        public int Puntaje { get; set; }

        // Aciertos / intentos, como porcentaje de 0 a 100 (no de 0 a 1).
        [BsonElement("precision")]
        [Range(0.0, 100.0)]
        public double Precision { get; set; }

        [BsonElement("tiempoSegundos")]
        [Range(0, int.MaxValue)]
        public int TiempoSegundos { get; set; }

        // La pone el servidor al recibir la sesión (ver SesionesController.Post).
        [BsonElement("fecha")]
        public string Fecha { get; set; } = "";

        // ---------- Métricas nuevas para el informe detallado ----------

        [BsonElement("aciertos")]
        [Range(0, int.MaxValue)]
        public int Aciertos { get; set; }

        [BsonElement("errores")]
        [Range(0, int.MaxValue)]
        public int Errores { get; set; }

        // "baja", "media" o "alta"
        [BsonElement("dificultad")]
        public string Dificultad { get; set; } = "";

        // false si el paciente abandonó la sesión. Las sesiones viejas, que no traen
        // este campo, se consideran completadas.
        [BsonElement("completada")]
        public bool Completada { get; set; } = true;

        [BsonElement("notas")]
        public string Notas { get; set; } = "";
    }
}
