export interface Produto {
  id: number;
  sku: string;
  nome: string;
  precoBase: number;
  precoFinal: number;
  ativo: boolean;
  estoqueAtual: number;
  criadoEm: string;
  atualizadoEm?: string;
}

export interface ResultDto<T> {
  sucesso: boolean;
  dados: T;
  mensagem: string;
}
