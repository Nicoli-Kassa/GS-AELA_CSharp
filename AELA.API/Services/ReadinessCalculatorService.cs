using AELA.API.Models;

namespace AELA.API.Services
{
    // Implementa a interface 
    public class ReadinessCalculatorService : IReadinessCalculator
    {
        public ReadinessScore Calcular(Baseline baseline, LeituraFisiologica leitura, TipoTarefa tarefa)
        {
            // 1) Calcula o desvio percentual de cada métrica em relação ao baseline.
            var desvios = new Dictionary<string, double>
            {
                ["fc"] = DesvioPercentual(leitura.FrequenciaCardiaca, baseline.FrequenciaCardiacaBasal),
                ["reacao"] = DesvioPercentual(leitura.TempoReacao, baseline.TempoReacaoBasal),
                ["ocular"] = DesvioPercentual(leitura.PressaoOcular, baseline.PressaoOcularBasal)
            };

            // 2) Pesos dependem do tipo de tarefa  
            var pesos = ObterPesos(tarefa);

            // 3) Soma ponderada dos desvios ABSOLUTOS  
            double somaPonderada = 0;
            foreach (var metrica in desvios)
            {
                somaPonderada += Math.Abs(metrica.Value) * pesos[metrica.Key];
            }

            // 4) Score final entre 0 e 100.
            double score = Math.Clamp(100 - somaPonderada, 0, 100);

            return new ReadinessScore
            {
                OperadorId = leitura.OperadorId,
                Tarefa = tarefa,
                Score = score,
                PercentualDesvio = somaPonderada
            };
        }

        // Método coeso: uma única responsabilidade (calcular desvio).
        private static double DesvioPercentual(double atual, double valorBase)
        {
            if (valorBase == 0) return 0; // Evita divisão por zero
            return ((atual - valorBase) / valorBase) * 100;
        }

        // Switch sobre o enum: cada tarefa pesa as métricas de forma diferente.
        private static Dictionary<string, double> ObterPesos(TipoTarefa tarefa)
        {
            return tarefa switch
            {
                TipoTarefa.EVA => new() { ["fc"] = 0.5, ["reacao"] = 0.3, ["ocular"] = 0.2 },
                TipoTarefa.OperacaoCognitiva => new() { ["fc"] = 0.2, ["reacao"] = 0.6, ["ocular"] = 0.2 },
                TipoTarefa.TarefaMotora => new() { ["fc"] = 0.4, ["reacao"] = 0.4, ["ocular"] = 0.2 },
                _ => new() { ["fc"] = 0.34, ["reacao"] = 0.33, ["ocular"] = 0.33 }
            };
        }
    }
}