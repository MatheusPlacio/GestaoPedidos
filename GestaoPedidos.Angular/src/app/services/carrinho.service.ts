import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ItemCarrinho } from '../models/pedido.model';
import { Produto } from '../models/produto.model';

@Injectable({
  providedIn: 'root'
})
export class CarrinhoService {
  private itensCarrinho: ItemCarrinho[] = [];
  private itensCarrinhoSubject = new BehaviorSubject<ItemCarrinho[]>([]);

  itensCarrinho$ = this.itensCarrinhoSubject.asObservable();

  adicionarItem(produto: Produto, quantidade: number = 1) {
    const itemExistente = this.itensCarrinho.find(item => item.produto.id === produto.id);

    if (itemExistente) {
      itemExistente.quantidade += quantidade;
    } else {
      this.itensCarrinho.push({ produto, quantidade });
    }

    this.itensCarrinhoSubject.next([...this.itensCarrinho]);
  }

  removerItem(produtoId: number) {
    this.itensCarrinho = this.itensCarrinho.filter(item => item.produto.id !== produtoId);
    this.itensCarrinhoSubject.next([...this.itensCarrinho]);
  }

  atualizarQuantidade(produtoId: number, quantidade: number) {
    const item = this.itensCarrinho.find(item => item.produto.id === produtoId);
    if (item) {
      item.quantidade = quantidade;
      this.itensCarrinhoSubject.next([...this.itensCarrinho]);
    }
  }

  limparCarrinho() {
    this.itensCarrinho = [];
    this.itensCarrinhoSubject.next([]);
  }

  obterTotal(): number {
    return this.itensCarrinho.reduce((total, item) =>
      total + ((item.produto.precoFinal || item.produto.precoBase) * item.quantidade), 0
    );
  }

  obterQuantidadeTotal(): number {
    return this.itensCarrinho.reduce((total, item) => total + item.quantidade, 0);
  }
}
