USE db_Vilela_Doces
GO

ALTER PROCEDURE p_RegistraPedido
    @total money, 
    @id_cliente int, 
    @id_endereco int,
    @observacoes VARCHAR(200) = NULL,  
    @frete MONEY = 0.00,
    @forma_pagamento INT, -- [RESTORED] Parâmetro trazido de volta
    @data_entrega_agendada DATETIME = NULL, 
    @opcao_pagamento_encomenda INT = 0 
AS 
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID_Pedido INT;
    DECLARE @Data_Hora_Pedido DATETIME = GETDATE(); 
    DECLARE @PrazoFinal DATETIME;
    DECLARE @MetadeValor MONEY;

    BEGIN TRY
        BEGIN TRANSACTION

        -- 1. Define o prazo de entrega
        IF @data_entrega_agendada IS NOT NULL
            SET @PrazoFinal = @data_entrega_agendada;
        ELSE
            SET @PrazoFinal = DATEADD(day, 1, @Data_Hora_Pedido);

        -- 2. Insere o Pedido
        INSERT INTO Pedidos (total, frete, observacoes, data_hora, status, id_cliente)
        VALUES (@total, @frete, @observacoes, @Data_Hora_Pedido, 0, @id_cliente);
        
        SET @ID_Pedido = SCOPE_IDENTITY(); 

        -- 3. Insere a Entrega
        INSERT INTO Entregas (data_prazo, status, id_pedido, id_endereco)
        VALUES (@PrazoFinal, 0, @ID_Pedido, @id_endereco);

        -- 4. Lógica de Pagamentos
        
        -- CENÁRIO A: Encomenda - Tudo no Pix agora
        IF @opcao_pagamento_encomenda = 1
        BEGIN
             INSERT INTO Pagamentos (forma_pagamento, status, data_limite, valor_pago, id_pedido)
             VALUES (1, 0, DATEADD(MINUTE, 30, @Data_Hora_Pedido), @total, @ID_Pedido); 
        END
        
        -- CENÁRIO B: Encomenda Parcelada (50% Sinal + 50% Final)
        ELSE IF @opcao_pagamento_encomenda IN (2, 3)
        BEGIN
            SET @MetadeValor = @total / 2;

            -- Parcela 1: O SINAL (Sempre Pix agora)
            INSERT INTO Pagamentos (forma_pagamento, status, data_limite, valor_pago, id_pedido)
            VALUES (1, 0, DATEADD(MINUTE, 30, @Data_Hora_Pedido), @MetadeValor, @ID_Pedido);

            -- Parcela 2: O RESTANTE (Na data da entrega)
            DECLARE @FormaPagtoSegundaParcela INT;
            
            IF @opcao_pagamento_encomenda = 2 
                SET @FormaPagtoSegundaParcela = 1; -- Pix
            ELSE 
                SET @FormaPagtoSegundaParcela = 0; -- Dinheiro

            INSERT INTO Pagamentos (forma_pagamento, status, data_limite, valor_pago, id_pedido)
            VALUES (@FormaPagtoSegundaParcela, 0, @PrazoFinal, @MetadeValor, @ID_Pedido);
        END

        -- CENÁRIO C: Pronta Entrega (Fluxo Original)
        ELSE
        BEGIN
            -- [CORREÇÃO] Aqui usamos a @forma_pagamento que vem do parâmetro
             INSERT INTO Pagamentos (forma_pagamento, status, data_limite, valor_pago, id_pedido)
             VALUES (@forma_pagamento, 0, @PrazoFinal, @total, @ID_Pedido);
        END

        COMMIT TRANSACTION
        SELECT @ID_Pedido
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        RETURN 0
    END CATCH
END
GO