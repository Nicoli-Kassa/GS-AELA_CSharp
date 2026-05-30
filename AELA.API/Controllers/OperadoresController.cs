using AELA.API.Data;
using AELA.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AELA.API.Controllers
{
    [ApiController]
    [Route("api/operadores")]
    public class OperadoresController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IReadinessCalculator _calculator;

        // Injeção de dependência: recebe o banco e o calculador prontos.
        public OperadoresController(AppDbContext db, IReadinessCalculator calculator)
        {
            _db = db;
            _calculator = calculator;
        }

        // ---------- ASTRONAUTAS ----------

        // POST /api/operadores/astronautas → Cadastra um astronauta
        [HttpPost("astronautas")]
        public async Task<IActionResult> CadastrarAstronauta([FromBody] Astronauta operador)
        {
            _db.Astronautas.Add(operador);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterAstronautaPorId), new { id = operador.Id }, operador);
        }

        // GET /api/operadores/astronautas → Lista todos os astronautas
        [HttpGet("astronautas")]
        public async Task<IActionResult> ListarAstronautas()
        {
            var operadores = await _db.Astronautas
                .OrderBy(a => a.Id)
                .ToListAsync();
            return Ok(operadores);
        }

        // GET /api/operadores/astronautas/{id} → Obtém um astronauta específico
        [HttpGet("astronautas/{id}")]
        public async Task<IActionResult> ObterAstronautaPorId(int id)
        {
            var operador = await _db.Astronautas.FindAsync(id);
            if (operador == null) return NotFound($"Astronauta {id} não encontrado.");
            return Ok(operador);
        }

        // ---------- TERRESTRES ----------

        // POST /api/operadores/terrestres → Cadastra um operador terrestre (bombeiro, alpinista etc.)
        [HttpPost("terrestres")]
        public async Task<IActionResult> CadastrarTerrestre([FromBody] OperadorTerrestre operador)
        {
            _db.OperadoresTerrestres.Add(operador);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterTerrestrePorId), new { id = operador.Id }, operador);
        }

        // GET /api/operadores/terrestres → Lista todos os operadores terrestres
        [HttpGet("terrestres")]
        public async Task<IActionResult> ListarTerrestres()
        {
            var operadores = await _db.OperadoresTerrestres
                .OrderBy(o => o.Id)
                .ToListAsync();
            return Ok(operadores);
        }

        // GET /api/operadores/terrestres/{id} → Obtém um operador terrestre específico
        [HttpGet("terrestres/{id}")]
        public async Task<IActionResult> ObterTerrestrePorId(int id)
        {
            var operador = await _db.OperadoresTerrestres.FindAsync(id);
            if (operador == null) return NotFound($"Operador terrestre {id} não encontrado.");
            return Ok(operador);
        }

        // ---------- BASELINE / LEITURAS / SCORE (por id de astronauta) ----------

        // POST /api/operadores/{id}/baseline → Registra ou atualiza o baseline
        [HttpPost("{id}/baseline")]
        public async Task<IActionResult> RegistrarBaseline(int id, [FromBody] Baseline baseline)
        {
            var existe = await _db.Astronautas.CountAsync(a => a.Id == id) > 0;
            if (!existe) return NotFound($"Operador {id} não encontrado.");

            baseline.OperadorId = id;

            // Se já houver baseline, substitui; senão cria.
            var atual = await _db.Baselines.FirstOrDefaultAsync(b => b.OperadorId == id);
            if (atual != null) _db.Baselines.Remove(atual);

            _db.Baselines.Add(baseline);
            await _db.SaveChangesAsync();
            return Ok(baseline);
        }

        // POST /api/operadores/{id}/leituras → Registra nova leitura
        [HttpPost("{id}/leituras")]
        public async Task<IActionResult> RegistrarLeitura(int id, [FromBody] LeituraFisiologica leitura)
        {
            var existe = await _db.Astronautas.CountAsync(a => a.Id == id) > 0;
            if (!existe) return NotFound($"Operador {id} não encontrado.");

            leitura.OperadorId = id;
            leitura.Timestamp = DateTime.UtcNow; // Garante o carimbo de tempo
            _db.Leituras.Add(leitura);
            await _db.SaveChangesAsync();
            return Ok(leitura);
        }

        // GET /api/operadores/{id}/readiness → Calcula o score atual
        [HttpGet("{id}/readiness")]
        public async Task<IActionResult> ObterReadiness(int id, [FromQuery] TipoTarefa tarefa = TipoTarefa.EVA)
        {
            var baseline = await _db.Baselines.FirstOrDefaultAsync(b => b.OperadorId == id);
            if (baseline == null) return NotFound("Baseline não registrado para este operador.");

            // pega a leitura mais recente
            var leitura = await _db.Leituras
                .Where(l => l.OperadorId == id)
                .OrderByDescending(l => l.Timestamp)
                .FirstOrDefaultAsync();
            if (leitura == null) return NotFound("Nenhuma leitura registrada para este operador.");

            var score = _calculator.Calcular(baseline, leitura, tarefa);
            _db.Scores.Add(score);
            await _db.SaveChangesAsync();
            return Ok(score);
        }

        // GET /api/operadores/{id}/historico → Histórico de scores/desvios
        [HttpGet("{id}/historico")]
        public async Task<IActionResult> ObterHistorico(int id)
        {
            var historico = await _db.Scores
                .Where(s => s.OperadorId == id)
                .OrderByDescending(s => s.CalcularEm)
                .ToListAsync();
            return Ok(historico);
        }
    }
}