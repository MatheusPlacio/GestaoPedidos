import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto, ResultDto } from '../models/produto.model';
import { Pedido, CriarPedido } from '../models/pedido.model';
import { Promocao, CriarPromocao, AtualizarPromocao } from '../models/promocao.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Produtos
  obterProdutos(): Observable<ResultDto<Produto[]>> {
    return this.http.get<ResultDto<Produto[]>>(`${this.baseUrl}/Produto`);
  }

  criarProduto(produto: any): Observable<ResultDto<number>> {
    return this.http.post<ResultDto<number>>(`${this.baseUrl}/Produto`, produto);
  }

  atualizarProduto(id: number, produto: any): Observable<ResultDto<any>> {
    return this.http.put<ResultDto<any>>(`${this.baseUrl}/Produto/${id}`, produto);
  }

  excluirProduto(id: number): Observable<ResultDto<any>> {
    return this.http.delete<ResultDto<any>>(`${this.baseUrl}/Produto/${id}`);
  }

  // Pedidos
  obterPedidos(): Observable<ResultDto<Pedido[]>> {
    return this.http.get<ResultDto<Pedido[]>>(`${this.baseUrl}/Pedido`);
  }

  criarPedido(pedido: CriarPedido): Observable<ResultDto<number>> {
    return this.http.post<ResultDto<number>>(`${this.baseUrl}/Pedido`, pedido);
  }

  obterPedidoPorId(id: number): Observable<ResultDto<Pedido>> {
    return this.http.get<ResultDto<Pedido>>(`${this.baseUrl}/Pedido/${id}`);
  }

  confirmarPedido(id: number): Observable<ResultDto<any>> {
    return this.http.put<ResultDto<any>>(`${this.baseUrl}/Pedido/${id}/confirmar`, {});
  }

  cancelarPedido(id: number): Observable<ResultDto<any>> {
    return this.http.put<ResultDto<any>>(`${this.baseUrl}/Pedido/${id}/cancelar`, {});
  }

  // Promoções
  obterPromocoes(): Observable<ResultDto<Promocao[]>> {
    return this.http.get<ResultDto<Promocao[]>>(`${this.baseUrl}/Promocao`);
  }

  criarPromocao(promocao: CriarPromocao): Observable<ResultDto<number>> {
    return this.http.post<ResultDto<number>>(`${this.baseUrl}/Promocao`, promocao);
  }

  atualizarPromocao(id: number, promocao: AtualizarPromocao): Observable<ResultDto<any>> {
    return this.http.put<ResultDto<any>>(`${this.baseUrl}/Promocao/${id}`, promocao);
  }

  excluirPromocao(id: number): Observable<ResultDto<any>> {
    return this.http.delete<ResultDto<any>>(`${this.baseUrl}/Promocao/${id}`);
  }
}
