-- 1) Obter todas as promoções
CREATE OR ALTER PROCEDURE sp_ObterTodasPromocoes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, Descricao, ProdutoId, Tipo, Valor,
           DataInicio, DataFim, Ativo, CriadoEm
    FROM Promocoes
    ORDER BY Nome;
END
GO

-- 2) Obter promoção por Id
CREATE OR ALTER PROCEDURE sp_ObterPromocaoPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, Descricao, ProdutoId, Tipo, Valor,
           DataInicio, DataFim, Ativo, CriadoEm
    FROM Promocoes
    WHERE Id = @Id;
END
GO

-- 3) Criar nova promoção
CREATE OR ALTER PROCEDURE sp_CriarPromocao
    @Nome NVARCHAR(200),
    @Descricao NVARCHAR(500) = NULL,
    @ProdutoId INT,
    @Tipo INT,
    @Valor DECIMAL(18,2),
    @DataInicio DATETIME,
    @DataFim DATETIME,
    @Ativo BIT,
    @CriadoEm DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Promocoes (Nome, Descricao, ProdutoId, Tipo, Valor,
                           DataInicio, DataFim, Ativo, CriadoEm)
    VALUES (@Nome, @Descricao, @ProdutoId, @Tipo, @Valor,
            @DataInicio, @DataFim, @Ativo, @CriadoEm);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NovoId;
END
GO

-- 4) Atualizar promoção
CREATE OR ALTER PROCEDURE sp_AtualizarPromocao
    @Id INT,
    @Nome NVARCHAR(200),
    @Descricao NVARCHAR(500) = NULL,
    @ProdutoId INT,
    @Tipo INT,
    @Valor DECIMAL(18,2),
    @DataInicio DATETIME,
    @DataFim DATETIME,
    @Ativo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Promocoes
    SET Nome = @Nome,
        Descricao = @Descricao,
        ProdutoId = @ProdutoId,
        Tipo = @Tipo,
        Valor = @Valor,
        DataInicio = @DataInicio,
        DataFim = @DataFim,
        Ativo = @Ativo
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS LinhasAfetadas;
END
GO

-- 5) Excluir promoção
CREATE OR ALTER PROCEDURE sp_ExcluirPromocao
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Promocoes
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS LinhasAfetadas;
END
GO

-- 6) Obter promoções por produto (ativas e no período válido)
CREATE OR ALTER PROCEDURE sp_ObterPromocoesPorProduto
    @ProdutoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, Descricao, ProdutoId, Tipo, Valor,
           DataInicio, DataFim, Ativo, CriadoEm
    FROM Promocoes
    WHERE ProdutoId = @ProdutoId
      AND Ativo = 1
      AND GETDATE() BETWEEN DataInicio AND DataFim
    ORDER BY Nome;
END
GO
