namespace AELA.API.Models
{
    // Classe ABSTRATA | Todo Operador (astronauta ou terrestre) compartilha estes campos
    public abstract class OperadorBase
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public string TipoAmbiente { get; set; } = string.Empty;
    }
}
