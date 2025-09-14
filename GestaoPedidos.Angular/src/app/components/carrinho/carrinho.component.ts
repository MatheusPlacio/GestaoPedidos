import { Component, OnInit } from '@angular/core';
import { CarrinhoService } from '../../services/carrinho.service';
import { ApiService } from '../../services/api.service';
import { ItemCarrinho, CriarPedido } from '../../models/pedido.model';

@Component({
  selector: 'app-carrinho',
  templateUrl: './carrinho.component.html',
  styleUrls: ['./carrinho.component.css']
})
export class CarrinhoComponent implements OnInit {
  itens: ItemCarrinho[] = [];
  clienteId = 1;
  criandoPedido = false;

  constructor(
    private carrinhoService: CarrinhoService,
    private apiService: ApiService
  ) { }

  ngOnInit() {
    this.carrinhoService.itensCarrinho$.subscribe(itens => {
      this.itens = itens;
    });
  }

  atualizarQuantidade(produtoId: number, event: any) {
    const quantidade = parseInt(event.target.value);
    if (quantidade > 0) {
      this.carrinhoService.atualizarQuantidade(produtoId, quantidade);
    }
  }

  removerItem(produtoId: number) {
    this.carrinhoService.removerItem(produtoId);
  }

  obterTotal(): number {
    return this.carrinhoService.obterTotal();
  }

  formatarPreco(preco: number): string {
    return preco.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  }

  finalizarPedido() {
    if (this.itens.length === 0) {
      alert('Carrinho vazio!');
      return;
    }

    this.criandoPedido = true;

    const pedido: CriarPedido = {
      clienteId: this.clienteId,
      itens: this.itens.map(item => ({
        produtoId: item.produto.id,
        quantidade: item.quantidade
      }))
    };

    this.apiService.criarPedido(pedido).subscribe({
      next: (resultado) => {
        if (resultado.sucesso) {
          alert(`Pedido criado com sucesso! ID: ${resultado.dados}`);
          this.carrinhoService.limparCarrinho();
        } else {
          alert('Erro ao criar pedido: ' + resultado.mensagem);
        }
        this.criandoPedido = false;
      },
      error: (error) => {
        alert('Erro ao criar pedido: ' + error.message);
        this.criandoPedido = false;
      }
    });
  }

  limparCarrinho() {
    if (confirm('Deseja limpar o carrinho?')) {
      this.carrinhoService.limparCarrinho();
    }
  }
}
