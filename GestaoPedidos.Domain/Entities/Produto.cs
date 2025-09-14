namespace GestaoPedidos.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; }
        public int EstoqueAtual { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }

        public decimal CalcularPrecoFinal(List<Promocao> promocoes)
        {
            decimal precoFinal = PrecoBase;
            
            foreach (var promocao in promocoes.Where(p => p.Ativo))
            {
                precoFinal = promocao.AplicarDesconto(precoFinal);
            }

            return precoFinal;
        }
    }
}
