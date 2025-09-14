using GestaoPedidos.Domain.Enums;

namespace GestaoPedidos.Application.DTOs
{
    public class PromocaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int ProdutoId { get; set; }
        public TipoPromocao Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
    }

    public class CriarPromocaoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int ProdutoId { get; set; }
        public TipoPromocao Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class AtualizarPromocaoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int ProdutoId { get; set; }
        public TipoPromocao Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; }
    }
}
