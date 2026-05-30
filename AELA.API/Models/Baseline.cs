namespace AELA.API.Models
{
    // O "zero" individual do operador: o estado fisiológico ideal de referência.
    // É contra ESTE baseline que toda leitura futura é comparada
    public class Baseline
    {
        public int Id { get; set; }
        public int OperadorId { get; set; }
        public double FrequenciaCardiacaBasal { get; set; }
        public double TempoReacaoBasal { get; set; }
        public double PressaoOcularBasal { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
    }
}
