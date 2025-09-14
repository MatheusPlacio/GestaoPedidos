using GestaoPedidos.Domain.Enums;

namespace GestaoPedidos.Domain.Entities
{
    public class Promocao
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

        public decimal AplicarDesconto(decimal precoOriginal)
        {
            if (!Ativo || DateTime.Now < DataInicio || DateTime.Now > DataFim)
                return precoOriginal;

            return Tipo switch
            {
                TipoPromocao.Porcentagem => precoOriginal * (1 - Valor / 100),
                TipoPromocao.ValorFixo => Math.Max(0, precoOriginal - Valor),
                _ => precoOriginal
            };
        }
    }
}
