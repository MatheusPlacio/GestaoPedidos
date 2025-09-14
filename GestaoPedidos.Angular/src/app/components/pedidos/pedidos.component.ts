import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Pedido, StatusPedido } from '../../models/pedido.model';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.css']
})
export class PedidosComponent implements OnInit {
  pedidos: Pedido[] = [];
  carregando = false;
  erro = '';
  pedidoDetalhes: Pedido | null = null;
  mostrarDetalhes = false;

  constructor(private apiService: ApiService) { }

  ngOnInit() {
    this.carregarPedidos();
  }

  carregarPedidos() {
    this.carregando = true;
    this.erro = '';

    this.apiService.obterPedidos().subscribe({
      next: (resultado) => {
        if (resultado.sucesso) {
          this.pedidos = resultado.dados;
        } else {
          this.erro = resultado.mensagem;
        }
        this.carregando = false;
      },
      error: (error) => {
        this.erro = 'Erro ao carregar pedidos: ' + error.message;
        this.carregando = false;
      }
    });
  }

  verDetalhes(pedido: Pedido) {
    this.apiService.obterPedidoPorId(pedido.id).subscribe({
      next: (resultado) => {
        if (resultado.sucesso) {
          this.pedidoDetalhes = resultado.dados;
          this.mostrarDetalhes = true;
        } else {
          alert('Erro ao carregar detalhes: ' + resultado.mensagem);
        }
      },
      error: (error) => {
        alert('Erro ao carregar detalhes: ' + error.message);
      }
    });
  }

  fecharDetalhes() {
    this.mostrarDetalhes = false;
    this.pedidoDetalhes = null;
  }

  formatarPreco(preco: number): string {
    return preco.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  }

  formatarData(data: string): string {
    return new Date(data).toLocaleString('pt-BR');
  }

  obterStatusTexto(status: StatusPedido): string {
    switch (status) {
      case StatusPedido.Rascunho: return 'Rascunho';
      case StatusPedido.Confirmado: return 'Confirmado';
      case StatusPedido.Cancelado: return 'Cancelado';
      case StatusPedido.Faturado: return 'Faturado';
      default: return 'Desconhecido';
    }
  }

  obterClasseStatus(status: StatusPedido): string {
    switch (status) {
      case StatusPedido.Rascunho: return 'status-rascunho';
      case StatusPedido.Confirmado: return 'status-confirmado';
      case StatusPedido.Cancelado: return 'status-cancelado';
      case StatusPedido.Faturado: return 'status-faturado';
      default: return '';
    }
  }

  podeConfirmar(pedido: Pedido): boolean {
    return pedido.status === StatusPedido.Rascunho;
  }

  podeCancelar(pedido: Pedido): boolean {
    return pedido.status === StatusPedido.Rascunho || pedido.status === StatusPedido.Confirmado;
  }

  confirmarPedido(pedido: Pedido) {
    if (confirm(`Deseja confirmar o pedido ${pedido.numero}?\n\nIsso irá validar o estoque e baixar os produtos.`)) {
      this.apiService.confirmarPedido(pedido.id).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            alert('Pedido confirmado com sucesso!');
            this.carregarPedidos();
          } else {
            alert('Erro ao confirmar pedido: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao confirmar pedido: ' + error.message);
        }
      });
    }
  }

  cancelarPedido(pedido: Pedido) {
    if (confirm(`Deseja cancelar o pedido ${pedido.numero}?\n\nEsta ação não pode ser desfeita.`)) {
      this.apiService.cancelarPedido(pedido.id).subscribe({
        next: (resultado) => {
          if (resultado.sucesso) {
            alert('Pedido cancelado com sucesso!');
            this.carregarPedidos();
          } else {
            alert('Erro ao cancelar pedido: ' + resultado.mensagem);
          }
        },
        error: (error) => {
          alert('Erro ao cancelar pedido: ' + error.message);
        }
      });
    }
  }
}
