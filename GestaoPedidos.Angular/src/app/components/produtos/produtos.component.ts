import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CarrinhoService } from '../../services/carrinho.service';
import { EventoService } from '../../services/evento.service';
import { Produto } from '../../models/produto.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-produtos',
  templateUrl: './produtos.component.html',
  styleUrls: ['./produtos.component.css']
})
export class ProdutosComponent implements OnInit, OnDestroy {
  produtos: Produto[] = [];
  carregando = false;
  erro = '';
  mostrarFormulario = false;
  produtoEdicao: Produto | null = null;
  private subscription: Subscription = new Subscription();

  formulario = {
    sku: '',
    nome: '',
    precoBase: 0,
    ativo: true,
    estoqueAtual: 0
  };

  constructor(
    private apiService: ApiService,
    private carrinhoService: CarrinhoService,
    private eventoService: EventoService
  ) { }

  ngOnInit() {
    this.carregarProdutos();

    // Escutar eventos de atualização de promoções
    this.subscription.add(
      this.eventoService.atualizarProdutos$.subscribe(() => {
        this.carregarProdutos();
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  carregarProdutos() {
    this.carregando = true;
    this.erro = '';

    this.apiService.obterProdutos().subscribe({
      next: (resultado) => {
        if (resultado.sucesso) {
          this.produtos = resultado.dados;
        } else {
          this.erro = resultado.mensagem;
        }
        this.carregando = false;
      },
      error: (error) => {
        this.erro = 'Erro ao carregar produtos: ' + error.message;
        this.carregando = false;
      }
    });
  }

  adicionarAoCarrinho(produto: Produto) {
    this.carrinhoService.adicionarItem(produto, 1);
    alert(`${produto.nome} adicionado ao carrinho!`);
  }

  formatarPrecoFinal(produto: Produto): string {
    if (produto.precoFinal < produto.precoBase) {
      return `${this.formatarPreco(produto.precoFinal)} (era ${this.formatarPreco(produto.precoBase)})`;
    }
    return this.formatarPreco(produto.precoFinal);
  }

  formatarPreco(preco: number): string {
    return preco.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  }

  // CRUD Produtos
  mostrarFormularioNovo() {
    this.mostrarFormulario = true;
    this.produtoEdicao = null;
    this.limparFormulario();
  }

  editarProduto(produto: Produto) {
    this.mostrarFormulario = true;
    this.produtoEdicao = produto;
    this.formulario = {
      sku: produto.sku,
      nome: produto.nome,
      precoBase: produto.precoBase,
      ativo: produto.ativo,
      estoqueAtual: produto.estoqueAtual
    };
  }

  salvarProduto() {
    if (this.produtoEdicao) {
      // Atualizar
      const dto = {
        nome: this.formulario.nome,
        precoBase: this.formulario.precoBase,
        ativo: this.formulario.ativo,
        estoqueAtual: this.formulario.estoqueAtual
      };

      this.apiService.atualizarProduto(this.produtoEdicao.id, dto).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            this.carregarProdutos();
            this.cancelarFormulario();
            alert('Produto atualizado com sucesso!');
          } else {
            alert('Erro: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao atualizar produto: ' + error.message);
        }
      });
    } else {
      // Criar novo
      const dto = {
        sku: this.formulario.sku,
        nome: this.formulario.nome,
        precoBase: this.formulario.precoBase,
        ativo: this.formulario.ativo,
        estoqueAtual: this.formulario.estoqueAtual
      };

      this.apiService.criarProduto(dto).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            this.carregarProdutos();
            this.cancelarFormulario();
            alert('Produto criado com sucesso!');
          } else {
            alert('Erro: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao criar produto: ' + error.message);
        }
      });
    }
  }

  excluirProduto(produto: Produto) {
    if (confirm(`Deseja excluir o produto ${produto.nome}?`)) {
      this.apiService.excluirProduto(produto.id).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            this.carregarProdutos();
            alert('Produto excluído com sucesso!');
          } else {
            alert('Erro: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao excluir produto: ' + error.message);
        }
      });
    }
  }

  cancelarFormulario() {
    this.mostrarFormulario = false;
    this.produtoEdicao = null;
    this.limparFormulario();
  }

  private limparFormulario() {
    this.formulario = {
      sku: '',
      nome: '',
      precoBase: 0,
      ativo: true,
      estoqueAtual: 0
    };
  }
}
