namespace AELA.API.Models
{
    // Medição pontual feita durante a missão
    public class LeituraFisiologica
    {
        public int Id { get; set; }
        public int OperadorId { get; set; }
        public double FrequenciaCardiaca { get; set; }
        public double TempoReacao { get; set; }
        public double PressaoOcular { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
