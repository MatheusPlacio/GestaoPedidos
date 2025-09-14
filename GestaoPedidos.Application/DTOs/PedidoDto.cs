using GestaoPedidos.Domain.Enums;

namespace GestaoPedidos.Application.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public StatusPedido Status { get; set; }
        public string StatusDescricao { get; set; } = string.Empty;
        public decimal TotalBruto { get; set; }
        public decimal Desconto { get; set; }
        public decimal TotalLiquido { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? ConfirmadoEm { get; set; }
        public DateTime? CanceladoEm { get; set; }
        public DateTime? FaturadoEm { get; set; }
        public List<ItemPedidoDto> Itens { get; set; } = new List<ItemPedidoDto>();
    }

    public class ItemPedidoDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string ProdutoSku { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal DescontoItem { get; set; }
        public decimal TotalItem { get; set; }
    }

    public class CriarPedidoDto
    {
        public int ClienteId { get; set; }
        public List<CriarItemPedidoDto> Itens { get; set; } = new List<CriarItemPedidoDto>();
    }

    public class CriarItemPedidoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
