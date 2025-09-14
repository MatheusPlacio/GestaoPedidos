import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProdutosComponent } from './components/produtos/produtos.component';
import { PromocoesComponent } from './components/promocoes/promocoes.component';
import { CarrinhoComponent } from './components/carrinho/carrinho.component';
import { PedidosComponent } from './components/pedidos/pedidos.component';

const routes: Routes = [
  { path: '', redirectTo: '/produtos', pathMatch: 'full' },
  { path: 'produtos', component: ProdutosComponent },
  { path: 'promocoes', component: PromocoesComponent },
  { path: 'carrinho', component: CarrinhoComponent },
  { path: 'pedidos', component: PedidosComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
