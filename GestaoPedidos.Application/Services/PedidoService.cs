using GestaoPedidos.Application.DTOs;
using GestaoPedidos.Application.DTOs.Common;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Enums;
using GestaoPedidos.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace GestaoPedidos.Application.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPromocaoRepository _promocaoRepository;

        public PedidoService(
            IPedidoRepository pedidoRepository,
            IProdutoRepository produtoRepository,
            IPromocaoRepository promocaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
            _promocaoRepository = promocaoRepository;
        }

        public async Task<ResultDto<List<PedidoDto>>> ObterTodosAsync()
        {
            try
            {
                var pedidos = await _pedidoRepository.ObterTodosAsync();
                var pedidosDto = new List<PedidoDto>();

                foreach (var pedido in pedidos)
                {
                    var itensDto = new List<ItemPedidoDto>();
                    
                    if (!string.IsNullOrEmpty(pedido.Itens))
                    {
                        var itens = JsonSerializer.Deserialize<List<ItemPedidoDto>>(pedido.Itens);
                        if (itens != null)
                        {
                            itensDto = itens;
                        }
                    }

                    pedidosDto.Add(new PedidoDto
                    {
                        Id = pedido.Id,
                        Numero = pedido.Numero,
                        ClienteId = pedido.ClienteId,
                        Status = pedido.Status,
                        StatusDescricao = pedido.Status.ToString(),
                        TotalBruto = pedido.TotalBruto,
                        Desconto = pedido.Desconto,
                        TotalLiquido = pedido.TotalLiquido,
                        CriadoEm = pedido.CriadoEm,
                        ConfirmadoEm = pedido.ConfirmadoEm,
                        CanceladoEm = pedido.CanceladoEm,
                        FaturadoEm = pedido.FaturadoEm,
                        Itens = itensDto
                    });
                }

                return ResultDto<List<PedidoDto>>.SucessoComDados(pedidosDto);
            }
            catch (Exception ex)
            {
                return ResultDto<List<PedidoDto>>.Erro($"Erro ao obter pedidos: {ex.Message}");
            }
        }

        public async Task<ResultDto<PedidoDto>> ObterPorIdAsync(int id)
        {
            try
            {
                var pedido = await _pedidoRepository.ObterPorIdAsync(id);
                if (pedido == null)
                    return ResultDto<PedidoDto>.Erro("Pedido não encontrado");

                var itensDto = new List<ItemPedidoDto>();
                
                if (!string.IsNullOrEmpty(pedido.Itens))
                {
                    var itens = JsonSerializer.Deserialize<List<ItemPedidoDto>>(pedido.Itens);
                    if (itens != null)
                    {
                        itensDto = itens;
                    }
                }

                var pedidoDto = new PedidoDto
                {
                    Id = pedido.Id,
                    Numero = pedido.Numero,
                    ClienteId = pedido.ClienteId,
                    Status = pedido.Status,
                    StatusDescricao = pedido.Status.ToString(),
                    TotalBruto = pedido.TotalBruto,
                    Desconto = pedido.Desconto,
                    TotalLiquido = pedido.TotalLiquido,
                    CriadoEm = pedido.CriadoEm,
                    ConfirmadoEm = pedido.ConfirmadoEm,
                    CanceladoEm = pedido.CanceladoEm,
                    FaturadoEm = pedido.FaturadoEm,
                    Itens = itensDto
                };

                return ResultDto<PedidoDto>.SucessoComDados(pedidoDto);
            }
            catch (Exception ex)
            {
                return ResultDto<PedidoDto>.Erro($"Erro ao obter pedido: {ex.Message}");
            }
        }

        public async Task<ResultDto<int>> CriarAsync(CriarPedidoDto dto)
        {
            try
            {
                // Validar disponibilidade de estoque e criar itens
                var itensDto = new List<ItemPedidoDto>();
                decimal totalBruto = 0;
                decimal totalDesconto = 0;

                foreach (var itemDto in dto.Itens)
                {
                    var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId);
                    if (produto == null)
                        return ResultDto<int>.Erro($"Produto {itemDto.ProdutoId} não encontrado");

                    if (!produto.Ativo)
                        return ResultDto<int>.Erro($"Produto {produto.Nome} não está ativo");

                    if (produto.EstoqueAtual < itemDto.Quantidade)
                        return ResultDto<int>.Erro($"Estoque insuficiente para o produto {produto.Nome}. Disponível: {produto.EstoqueAtual}");

                    // Buscar promoções e calcular preços
                    var promocoes = await _promocaoRepository.ObterPromocoesPorProduto(itemDto.ProdutoId);
                    var precoUnitario = produto.CalcularPrecoFinal(promocoes);
                    var subtotal = precoUnitario * itemDto.Quantidade;
                    var desconto = (produto.PrecoBase - precoUnitario) * itemDto.Quantidade;

                    var itemPedido = new ItemPedidoDto
                    {
                        Id = 0,
                        ProdutoId = itemDto.ProdutoId,
                        ProdutoNome = produto.Nome,
                        ProdutoSku = produto.Sku,
                        Quantidade = itemDto.Quantidade,
                        PrecoUnitario = precoUnitario,
                        DescontoItem = desconto,
                        TotalItem = subtotal
                    };

                    itensDto.Add(itemPedido);
                    totalBruto += produto.PrecoBase * itemDto.Quantidade;
                    totalDesconto += desconto;
                }

                // Criar o pedido
                var numero = await _pedidoRepository.GerarProximoNumero();
                var pedido = new Pedido
                {
                    Numero = numero,
                    ClienteId = dto.ClienteId,
                    Status = StatusPedido.Rascunho,
                    Itens = JsonSerializer.Serialize(itensDto),
                    TotalBruto = totalBruto,
                    Desconto = totalDesconto,
                    TotalLiquido = totalBruto - totalDesconto,
                    CriadoEm = DateTime.Now
                };

                var pedidoId = await _pedidoRepository.CriarAsync(pedido);
                return ResultDto<int>.SucessoComDados(pedidoId, "Pedido criado com sucesso");
            }
            catch (Exception ex)
            {
                return ResultDto<int>.Erro($"Erro ao criar pedido: {ex.Message}");
            }
        }

        public async Task<ResultDto> ConfirmarAsync(int id)
        {
            try
            {
                var pedido = await _pedidoRepository.ObterPorIdAsync(id);
                if (pedido == null)
                    return ResultDto.Erro("Pedido não encontrado");

                if (pedido.Status != StatusPedido.Rascunho)
                    return ResultDto.Erro("Apenas pedidos em rascunho podem ser confirmados");

                // Obter itens do JSON
                if (string.IsNullOrEmpty(pedido.Itens))
                    return ResultDto.Erro("Pedido sem itens");

                var itens = JsonSerializer.Deserialize<List<ItemPedidoDto>>(pedido.Itens);
                if (itens == null || !itens.Any())
                    return ResultDto.Erro("Pedido sem itens válidos");

                // Verificar estoque 
                foreach (var item in itens)
                {
                    var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId);
                    if (produto == null || produto.EstoqueAtual < item.Quantidade)
                        return ResultDto.Erro($"Estoque insuficiente para o produto {item.ProdutoNome}");
                }

                // Baixar estoque dos produtos
                foreach (var item in itens)
                {
                    await _produtoRepository.BaixarEstoque(item.ProdutoId, item.Quantidade);
                }

                var sucesso = await _pedidoRepository.ConfirmarPedido(id);
                return sucesso ? ResultDto.Ok("Pedido confirmado com sucesso") : ResultDto.Erro("Erro ao confirmar pedido");
            }
            catch (Exception ex)
            {
                return ResultDto.Erro($"Erro ao confirmar pedido: {ex.Message}");
            }
        }

        public async Task<ResultDto> CancelarAsync(int id)
        {
            try
            {
                var pedido = await _pedidoRepository.ObterPorIdAsync(id);
                if (pedido == null)
                    return ResultDto.Erro("Pedido não encontrado");

                if (pedido.Status == StatusPedido.Faturado)
                    return ResultDto.Erro("Pedidos faturados não podem ser cancelados");

                // Se estava confirmado, estornar estoque
                if (pedido.Status == StatusPedido.Confirmado)
                {
                    if (!string.IsNullOrEmpty(pedido.Itens))
                    {
                        var itens = JsonSerializer.Deserialize<List<ItemPedidoDto>>(pedido.Itens);
                        if (itens != null)
                        {
                            foreach (var item in itens)
                            {
                                await _produtoRepository.EstornarEstoque(item.ProdutoId, item.Quantidade);
                            }
                        }
                    }
                }

                var sucesso = await _pedidoRepository.CancelarPedido(id);
                return sucesso ? ResultDto.Ok("Pedido cancelado com sucesso") : ResultDto.Erro("Erro ao cancelar pedido");
            }
            catch (Exception ex)
            {
                return ResultDto.Erro($"Erro ao cancelar pedido: {ex.Message}");
            }
        }
    }
}
