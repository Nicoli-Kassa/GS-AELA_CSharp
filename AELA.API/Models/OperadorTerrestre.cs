namespace AELA.API.Models
{
    // HERANÇA: outra especialização  
    public class OperadorTerrestre : OperadorBase
    {
        // ex: "bombeiro", "alpinista", "mergulhador"
        public string TipoOperacao { get; set; } = string.Empty;
    }
}
