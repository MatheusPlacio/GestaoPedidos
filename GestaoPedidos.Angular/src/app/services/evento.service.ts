import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  private atualizarProdutosSubject = new Subject<void>();

  // Observable que outros componentes podem se inscrever
  atualizarProdutos$ = this.atualizarProdutosSubject.asObservable();

  // Método para emitir evento de atualização
  emitirAtualizacaoProdutos() {
    this.atualizarProdutosSubject.next();
  }
}
