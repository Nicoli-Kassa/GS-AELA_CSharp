namespace AELA.API.Models
{
    // HERANÇA: Astronauta é um OperadorBase e adiciona campos próprios
    public class Astronauta : OperadorBase
    {
        public string MissaoAtual { get; set; } = string.Empty;

        public int DiasEmOrbita { get; set; }
    }
}
