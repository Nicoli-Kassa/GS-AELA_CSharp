namespace AELA.API.Models
{
    // INTERFACE: o "contrato" do cálculo 
    public interface IReadinessCalculator
    {
        ReadinessScore Calcular(Baseline baseline, LeituraFisiologica leitura, TipoTarefa tarefa);
    }
}
