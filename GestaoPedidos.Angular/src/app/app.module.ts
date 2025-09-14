import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CarrinhoComponent } from './components/carrinho/carrinho.component';
import { PedidosComponent } from './components/pedidos/pedidos.component';
import { ProdutosComponent } from './components/produtos/produtos.component';
import { PromocoesComponent } from './components/promocoes/promocoes.component';

@NgModule({
  declarations: [
    AppComponent,
    CarrinhoComponent,
    PedidosComponent,
    ProdutosComponent,
    PromocoesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
