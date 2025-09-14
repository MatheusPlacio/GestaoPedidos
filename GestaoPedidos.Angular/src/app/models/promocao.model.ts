export interface Promocao {
  id: number;
  nome: string;
  descricao: string;
  produtoId: number;
  tipo: TipoPromocao;
  valor: number;
  dataInicio: string;
  dataFim: string;
  ativo: boolean;
  criadoEm: string;
}

export interface CriarPromocao {
  nome: string;
  descricao: string;
  produtoId: number;
  tipo: TipoPromocao;
  valor: number;
  dataInicio: string;
  dataFim: string;
  ativo: boolean;
}

export interface AtualizarPromocao {
  nome: string;
  descricao: string;
  produtoId: number;
  tipo: TipoPromocao;
  valor: number;
  dataInicio: string;
  dataFim: string;
  ativo: boolean;
}

export enum TipoPromocao {
  Porcentagem = 1,
  ValorFixo = 2
}
