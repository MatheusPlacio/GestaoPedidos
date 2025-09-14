-- Script para criar as tabelas do sistema de Gestão de Pedidos
USE GestaoPedidos;
GO

-- Tabela de Produtos
CREATE TABLE Produtos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Sku NVARCHAR(50) NOT NULL UNIQUE,
    Nome NVARCHAR(200) NOT NULL,
    PrecoBase DECIMAL(18,2) NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1,
    EstoqueAtual INT NOT NULL DEFAULT 0,
    CriadoEm DATETIME2 NOT NULL DEFAULT GETDATE(),
    AtualizadoEm DATETIME2 NULL,
    
    CONSTRAINT CK_Produtos_PrecoBase CHECK (PrecoBase >= 0),
    CONSTRAINT CK_Produtos_EstoqueAtual CHECK (EstoqueAtual >= 0)
);

CREATE INDEX IX_Produtos_Sku ON Produtos (Sku);
CREATE INDEX IX_Produtos_Ativo ON Produtos (Ativo);

PRINT 'Tabela Produtos criada com sucesso!';
GO

-- Tabela de Promoções
CREATE TABLE Promocoes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(200) NOT NULL,
    Descricao NVARCHAR(500) NULL,
    ProdutoId INT NOT NULL, -- Referência direta ao produto
    Tipo INT NOT NULL, -- 1=Porcentagem, 2=ValorFixo
    Valor DECIMAL(18,2) NOT NULL,
    DataInicio DATETIME2 NOT NULL,
    DataFim DATETIME2 NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1,
    CriadoEm DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT CK_Promocoes_DataFim CHECK (DataFim > DataInicio),
    CONSTRAINT CK_Promocoes_Valor CHECK (Valor >= 0),
    CONSTRAINT CK_Promocoes_Tipo CHECK (Tipo IN (1, 2))
);

CREATE INDEX IX_Promocoes_ProdutoId ON Promocoes (ProdutoId);
CREATE INDEX IX_Promocoes_Ativo ON Promocoes (Ativo);
CREATE INDEX IX_Promocoes_DataVigencia ON Promocoes (DataInicio, DataFim);

PRINT 'Tabela Promocoes criada com sucesso!';
GO

-- Tabela de Pedidos
CREATE TABLE Pedidos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero NVARCHAR(20) NOT NULL UNIQUE,
    ClienteId INT NOT NULL,
    Status INT NOT NULL DEFAULT 1, -- 1=Rascunho, 2=Confirmado, 3=Cancelado, 4=Faturado
    Itens NVARCHAR(MAX) NULL, -- JSON com os itens do pedido
    TotalBruto DECIMAL(18,2) NOT NULL DEFAULT 0,
    Desconto DECIMAL(18,2) NOT NULL DEFAULT 0,
    TotalLiquido DECIMAL(18,2) NOT NULL DEFAULT 0,
    CriadoEm DATETIME2 NOT NULL DEFAULT GETDATE(),
    ConfirmadoEm DATETIME2 NULL,
    CanceladoEm DATETIME2 NULL,
    FaturadoEm DATETIME2 NULL,
    
    CONSTRAINT CK_Pedidos_TotalBruto CHECK (TotalBruto >= 0),
    CONSTRAINT CK_Pedidos_Desconto CHECK (Desconto >= 0),
    CONSTRAINT CK_Pedidos_TotalLiquido CHECK (TotalLiquido >= 0),
    CONSTRAINT CK_Pedidos_Status CHECK (Status IN (1, 2, 3, 4))
);

CREATE INDEX IX_Pedidos_Numero ON Pedidos (Numero);
CREATE INDEX IX_Pedidos_ClienteId ON Pedidos (ClienteId);
CREATE INDEX IX_Pedidos_Status ON Pedidos (Status);
CREATE INDEX IX_Pedidos_CriadoEm ON Pedidos (CriadoEm);

PRINT 'Tabela Pedidos criada com sucesso!';
GO


PRINT 'Todas as tabelas foram criadas com sucesso!';
