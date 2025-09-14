export interface Pedido {
  id: number;
  numero: string;
  clienteId: number;
  status: StatusPedido;
  statusDescricao: string;
  totalBruto: number;
  desconto: number;
  totalLiquido: number;
  criadoEm: string;
  confirmadoEm?: string;
  canceladoEm?: string;
  faturadoEm?: string;
  itens: ItemPedido[];
}

export interface ItemPedido {
  id: number;
  pedidoId: number;
  produtoId: number;
  produtoNome: string;
  produtoSku: string;
  quantidade: number;
  precoUnitario: number;
  descontoItem: number;
  totalItem: number;
}

export interface CriarPedido {
  clienteId: number;
  itens: CriarItemPedido[];
}

export interface CriarItemPedido {
  produtoId: number;
  quantidade: number;
}

export enum StatusPedido {
  Rascunho = 1,
  Confirmado = 2,
  Cancelado = 3,
  Faturado = 4
}

export interface ItemCarrinho {
  produto: any;
  quantidade: number;
}
