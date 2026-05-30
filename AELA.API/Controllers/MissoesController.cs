using AELA.API.Data;
using AELA.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AELA.API.Controllers
{
    [ApiController]
    [Route("api/missoes")]
    public class MissoesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IReadinessCalculator _calculator;

        public MissoesController(AppDbContext db, IReadinessCalculator calculator)
        {
            _db = db;
            _calculator = calculator;
        }

        // GET /api/missoes/{missaoId}/ranking?tarefa=EVA
        // Retorna a tripulação ordenada por prontidão para a tarefa.
        [HttpGet("{missaoId}/ranking")]
        public async Task<IActionResult> Ranking(string missaoId, [FromQuery] TipoTarefa tarefa = TipoTarefa.EVA)
        {
            // Operadores daquela missão.
            var operadores = await _db.Astronautas
                .Where(a => a.MissaoAtual == missaoId)
                .ToListAsync();

            if (operadores.Count == 0)
                return NotFound($"Nenhum operador na missão {missaoId}.");

            var ranking = new List<object>();

            foreach (var op in operadores)
            {
                var baseline = await _db.Baselines.FirstOrDefaultAsync(b => b.OperadorId == op.Id);
                var leitura = await _db.Leituras
                    .Where(l => l.OperadorId == op.Id)
                    .OrderByDescending(l => l.Timestamp)
                    .FirstOrDefaultAsync();

                // Só ranqueia quem tem dados suficientes
                if (baseline == null || leitura == null) continue;

                var score = _calculator.Calcular(baseline, leitura, tarefa);
                ranking.Add(new { op.Id, op.Nome, Score = score.Score, score.PercentualDesvio });
            }

            // Ordena do mais apto para o menos apto.
            var ordenado = ranking
                .OrderByDescending(r => ((dynamic)r).Score)
                .ToList();

            return Ok(new { missao = missaoId, tarefa = tarefa.ToString(), ranking = ordenado });
        }
    }
}