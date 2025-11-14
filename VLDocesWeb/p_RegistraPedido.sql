use db_Vilela_Doces
go

create PROCEDURE p_RegistraPedido
    @total money, 
    @id_cliente int, 
    @forma_pagamento int, 
    @id_endereco int,
    @observacoes VARCHAR(200) = NULL,  
    @frete MONEY = 0.00                
AS 
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID_Pedido INT;
    DECLARE @Data_Hora_Pedido DATETIME = GETDATE(); 
    DECLARE @data_limite_pagamento DATETIME;

    BEGIN TRY
        
        INSERT INTO Pedidos (total, frete, observacoes, data_hora, status, id_cliente)
        VALUES (@total, @frete, @observacoes, @Data_Hora_Pedido, 0, @id_cliente);
        
        SET @ID_Pedido = SCOPE_IDENTITY(); 

        IF @forma_pagamento = 0 
        BEGIN
            SET @data_limite_pagamento = DATEADD(day, 1, @Data_Hora_Pedido);
        END
        ELSE 
        BEGIN
            SET @data_limite_pagamento = DATEADD(MINUTE, 10, @Data_Hora_Pedido);
        END

        INSERT INTO Pagamentos (forma_pagamento, status, data_limite, valor_pago, id_pedido)
        VALUES (@forma_pagamento, 0, @data_limite_pagamento, @total, @ID_Pedido);

        INSERT INTO Entregas (data_prazo, status, id_pedido, id_endereco)
        VALUES (DATEADD(day, 1, @Data_Hora_Pedido), 0, @ID_Pedido, @id_endereco);

        SELECT @ID_Pedido
    END TRY
    BEGIN CATCH
        
        RETURN 0
    END CATCH
END
GO

DECLARE @Resultado INT;

EXEC @Resultado = p_RegistraPedido 
    @total = 47, 
    @id_cliente = 2, 
    @forma_pagamento = 1, 
    @id_endereco = 2;

-- Verifica o retorno
SELECT 
    CASE @Resultado
        WHEN 1 THEN 'SUCESSO: Pedido registrado e COMMIT realizado.'
        ELSE 'FALHA: Pedido falhou e ROLLBACK realizado.'
    END AS StatusExecucao;

-- Opcional: Verifique os dados inseridos
SELECT TOP 1 * FROM Pedidos ORDER BY id_pedido DESC;
SELECT TOP 1 * FROM Pagamentos ORDER BY id_pedido DESC;
SELECT TOP 1 * FROM Entregas ORDER BY id_pedido DESC;

