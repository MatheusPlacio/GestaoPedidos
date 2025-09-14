import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { EventoService } from '../../services/evento.service';
import { Promocao, TipoPromocao } from '../../models/promocao.model';

@Component({
  selector: 'app-promocoes',
  templateUrl: './promocoes.component.html',
  styleUrls: ['./promocoes.component.css']
})
export class PromocoesComponent implements OnInit {
  promocoes: Promocao[] = [];
  produtos: any[] = [];
  carregando = false;
  erro = '';
  mostrarFormulario = false;
  promocaoEdicao: Promocao | null = null;

  // Enum para template
  TipoPromocao = TipoPromocao;

  // Formulário
  formulario = {
    produtoId: null as number | null,
    descricao: '',
    tipo: TipoPromocao.Porcentagem,
    valor: 0,
    dataInicio: '',
    dataFim: '',
    ativo: true
  };

  constructor(
    private apiService: ApiService,
    private eventoService: EventoService
  ) { }

  ngOnInit() {
    this.carregarPromocoes();
    this.carregarProdutos();
  }

  carregarProdutos() {
    this.apiService.obterProdutos().subscribe({
      next: (resultado) => {
        if (resultado.sucesso) {
          this.produtos = resultado.dados;
        }
      },
      error: (error) => {
        console.error('Erro ao carregar produtos:', error);
      }
    });
  }

  carregarPromocoes() {
    this.carregando = true;
    this.erro = '';

    this.apiService.obterPromocoes().subscribe({
      next: (resultado) => {
        if (resultado.sucesso) {
          this.promocoes = resultado.dados;
        } else {
          this.erro = resultado.mensagem;
        }
        this.carregando = false;
      },
      error: (error) => {
        this.erro = 'Erro ao carregar promoções: ' + error.message;
        this.carregando = false;
      }
    });
  }

  mostrarFormularioNovo() {
    this.mostrarFormulario = true;
    this.promocaoEdicao = null;
    this.limparFormulario();
  }

  editarPromocao(promocao: Promocao) {
    this.mostrarFormulario = true;
    this.promocaoEdicao = promocao;

    // Buscar o produto vinculado à promoção pelo nome
    let produtoId = null;
    if (promocao.nome) {
      const produto = this.produtos.find(p => p.nome === promocao.nome);
      produtoId = produto ? produto.id : null;
    }

    this.formulario = {
      produtoId: produtoId,
      descricao: promocao.descricao,
      tipo: promocao.tipo,
      valor: promocao.valor,
      dataInicio: this.formatarDataParaInput(promocao.dataInicio),
      dataFim: this.formatarDataParaInput(promocao.dataFim),
      ativo: promocao.ativo
    };
  }

  salvarPromocao() {
    // Validações
    if (!this.formulario.produtoId) {
      alert('Por favor, selecione um produto para a promoção.');
      return;
    }

    if (!this.formulario.dataInicio) {
      alert('Por favor, selecione a data de início da promoção.');
      return;
    }

    if (!this.formulario.dataFim) {
      alert('Por favor, selecione a data de fim da promoção.');
      return;
    }

    if (new Date(this.formulario.dataInicio) >= new Date(this.formulario.dataFim)) {
      alert('A data de início deve ser anterior à data de fim.');
      return;
    }

    const produtoSelecionado = this.produtos.find(p => p.id === this.formulario.produtoId);
    const nomeProduto = produtoSelecionado ? produtoSelecionado.nome : '';

    if (this.promocaoEdicao) {
      // Atualizar
      const dto = {
        nome: nomeProduto,
        descricao: this.formulario.descricao,
        produtoId: this.formulario.produtoId,
        tipo: Number(this.formulario.tipo),
        valor: this.formulario.valor,
        dataInicio: new Date(this.formulario.dataInicio).toISOString(),
        dataFim: new Date(this.formulario.dataFim).toISOString(),
        ativo: this.formulario.ativo
      };

      this.apiService.atualizarPromocao(this.promocaoEdicao.id, dto).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            this.carregarPromocoes();
            this.carregarProdutos(); // Atualizar produtos para refletir mudanças de preço
            this.eventoService.emitirAtualizacaoProdutos(); // Notificar outros componentes
            this.cancelarFormulario();
            alert('Promoção atualizada com sucesso!');
          } else {
            alert('Erro: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao atualizar promoção: ' + error.message);
        }
      });
    } else {
      // Criar novo
      const dto = {
        nome: nomeProduto,
        descricao: this.formulario.descricao,
        produtoId: this.formulario.produtoId,
        tipo: Number(this.formulario.tipo),
        valor: this.formulario.valor,
        dataInicio: new Date(this.formulario.dataInicio).toISOString(),
        dataFim: new Date(this.formulario.dataFim).toISOString(),
        ativo: this.formulario.ativo
      };

      this.apiService.criarPromocao(dto).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            this.carregarPromocoes();
            this.carregarProdutos(); // Atualizar produtos para refletir mudanças de preço
            this.eventoService.emitirAtualizacaoProdutos(); // Notificar outros componentes
            this.cancelarFormulario();
            alert('Promoção criada com sucesso!');
          } else {
            alert('Erro: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao criar promoção: ' + error.message);
        }
      });
    }
  }

  excluirPromocao(promocao: Promocao) {
    if (confirm(`Deseja excluir a promoção ${promocao.nome}?`)) {
      this.apiService.excluirPromocao(promocao.id).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            this.carregarPromocoes();
            this.carregarProdutos(); // Atualizar produtos para refletir mudanças de preço
            this.eventoService.emitirAtualizacaoProdutos(); // Notificar outros componentes
            alert('Promoção excluída com sucesso!');
          } else {
            alert('Erro: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao excluir promoção: ' + error.message);
        }
      });
    }
  }

  cancelarFormulario() {
    this.mostrarFormulario = false;
    this.promocaoEdicao = null;
    this.limparFormulario();
  }

  private limparFormulario() {
    this.formulario = {
      produtoId: null,
      descricao: '',
      tipo: TipoPromocao.Porcentagem,
      valor: 0,
      dataInicio: '',
      dataFim: '',
      ativo: true
    };
  }

  private formatarDataParaInput(data: string): string {
    return new Date(data).toISOString().substring(0, 16);
  }

  formatarData(data: string): string {
    return new Date(data).toLocaleString('pt-BR');
  }

  obterDescricaoTipo(tipo: TipoPromocao): string {
    switch (tipo) {
      case TipoPromocao.Porcentagem:
        return 'Porcentagem';
      case TipoPromocao.ValorFixo:
        return 'Valor Fixo';
      default:
        return 'Desconhecido';
    }
  }

  formatarValor(promocao: Promocao): string {
    switch (promocao.tipo) {
      case TipoPromocao.Porcentagem:
        return `${promocao.valor}%`;
      case TipoPromocao.ValorFixo:
        return `R$ ${promocao.valor.toFixed(2)}`;
      default:
        return '';
    }
  }

  onTipoChange() {
    // Método mantido para compatibilidade, mas não faz nada agora
    // pois removemos o tipo LeveXPagueY
  }
}
