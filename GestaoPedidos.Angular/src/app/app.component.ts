import { Component, OnInit } from '@angular/core';
import { CarrinhoService } from './services/carrinho.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Gestão de Pedidos';
  abaSelecionada = 'produtos';
  quantidadeCarrinho = 0;

  constructor(private carrinhoService: CarrinhoService) {}

  ngOnInit() {
    this.carrinhoService.itensCarrinho$.subscribe(itens => {
      this.quantidadeCarrinho = this.carrinhoService.obterQuantidadeTotal();
    });
  }

  selecionarAba(aba: string) {
    this.abaSelecionada = aba;
  }
}
