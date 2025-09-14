# Sistema de Gestão de Pedidos

Sistema completo para gestão de produtos, promoções e pedidos, desenvolvido com arquitetura DDD no backend (.NET Core), SQL Server para persistência de dados e Angular para o frontend.

## 1. Banco de Dados

Para configurar o banco de dados, execute os scripts SQL na seguinte ordem:

1. `Scripts/00_CriarDatabase.sql` - Cria o banco de dados GestaoPedidos
2. `Scripts/01_CriarTabelas.sql` - Cria as tabelas do sistema
3. `Scripts/02_Procedures.sql` - Rode o script para a criação das procedures

Os scripts estão localizados na pasta `Scripts` do projeto.

## 2. Backend (.NET Core)

O backend foi desenvolvido seguindo os princípios de Domain-Driven Design (DDD), com uma arquitetura robusta dividida em camadas:

- **GestaoPedidos.Domain**: Entidades, enums e interfaces de repositórios
- **GestaoPedidos.Application**: DTOs, serviços de aplicação e lógica de negócio
- **GestaoPedidos.Infra**: Implementação dos repositórios e acesso a dados
- **GestaoPedidos.Api**: Controllers e configuração da API

### Tecnologias utilizadas

- **Dapper**: Micro ORM para acesso a dados de alta performance
- **SQL Server**: Banco de dados relacional
- **ASP.NET Core**: Framework web para APIs RESTful

### Implementação dos Repositórios

O projeto demonstra dois estilos de acesso a dados:

1. **Query Strings (PedidoRepository e ProdutoRepository)**: Consultas SQL diretamente no código
2. **Stored Procedures (PromocaoRepository)**: Chamadas a procedures armazenadas no banco de dados

Esta abordagem mista foi escolhida para demonstrar o conhecimento em ambas as técnicas.

### Configuração da Conexão

Antes de executar o projeto, altere a string de conexão no arquivo `GestaoPedidos.Api/appsettings.json` para apontar para seu servidor SQL Server local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=GestaoPedidos;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

## 3. Frontend (Angular)

Acesse a pasta GestaoPedidos.Angular com o comando cd GestaoPedidos.Angular. Em seguida, execute npm install e, depois, rode o projeto com o comando npm start.

### Fluxo Completo de Uso

Para testar o sistema, siga este fluxo que demonstra todas as funcionalidades:

1. **Gestão de Produtos**:
   - Acesse a aba "Produtos"
   - Crie ou edite produtos conforme necessário
   - Observe o estoque disponível

2. **Criação de Promoções**:
   - Acesse a aba "Promoções"
   - Crie uma promoção para um produto existente
   - Retorne à tela de produtos para verificar que o preço promocional foi aplicado

3. **Adição ao Carrinho**:
   - Na tela de produtos, clique em "Carrinho" para adicionar itens
   - Observe que os preços promocionais são aplicados automaticamente

4. **Finalização do Pedido**:
   - Na aba "Carrinho", finalize o pedido
   - O sistema criará um pedido em status de rascunho

5. **Confirmação do Pedido**:
   - Acesse "Meus Pedidos"
   - Confirme o pedido criado
   - Retorne à tela de produtos para verificar que o estoque foi reduzido automaticamente

6. **Cancelamento do Pedido**:
   - Em "Meus Pedidos", cancele o pedido confirmado
   - Retorne à tela de produtos para verificar que o estoque foi estornado.

Este fluxo demonstra a integração completa entre todas as partes do sistema, desde a criação de produtos e promoções até a gestão de pedidos e controle de estoque.

## Tecnologias Utilizadas

- **Backend**: .NET Core, C#, Dapper, SQL Server
- **Frontend**: Angular, TypeScript, HTML, CSS
- **Arquitetura**: Domain-Driven Design (DDD)
- **Banco de Dados**: SQL Server com modelagem relacional e stored procedures