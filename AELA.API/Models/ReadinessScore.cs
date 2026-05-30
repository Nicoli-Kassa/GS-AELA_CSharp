namespace AELA.API.Models
{
    // OUTPUT central do AELA: não é "você está saudável",
    // é "você está pronto para ESTA tarefa, agora"
    public class ReadinessScore
    {
        public int Id { get; set; }
        public int OperadorId { get; set; }
        public TipoTarefa Tarefa { get; set; }
        public double Score { get; set; } // 0 a 100
        public double PercentualDesvio { get; set; } // Quanto se afastou do baseline
        public DateTime CalcularEm { get; set; } = DateTime.UtcNow;
    }
}
