using GestaoPedidos.Domain.Enums;

namespace GestaoPedidos.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public StatusPedido Status { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal Desconto { get; set; }
        public decimal TotalLiquido { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? ConfirmadoEm { get; set; }
        public DateTime? CanceladoEm { get; set; }
        public DateTime? FaturadoEm { get; set; }

        public string? Itens { get; set; } // JSON dos itens
        
    }
}
