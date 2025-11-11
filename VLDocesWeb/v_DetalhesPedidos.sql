use db_Vilela_Doces
go

create view v_DetalhesPedidos
as 
select p.id_pedido Pedido, p.total Total, p.data_hora Data, 
      case p.status
        when 0 then 'Em análise'
        when 1 then 'Confirmado'
        when 2 then 'Cancelado'
        when 3 then 'Pronto p/ retirada'
        when 4 then 'Negado'
        when 5 then 'Finalizado'
      end StatusPedido,
      p.observacoes Observacoes, p.frete PrecoTeste, cli.id_pessoa IdCliente, 
      cli.nome Cliente, col.nome Colaborador, pr.nome_produto Produto, 
      pr.descricao Descricao, ip.preco_vend PrecoVendido, ip.quantidade Quantidade,
      ip.preco_vend * ip.quantidade PrecoItem, 
      case pag.forma_pagamento
        when 0 then 'Dinheiro'
        when 1 then 'Pix'
      end FormaPagamento,
      case pag.status
        when 0 then 'Pendente'
        when 1 then 'Pago'
        when 2 then 'Cancelado'
        when 3 then 'Estornado'
      end StatusPagamento,
      e.data_prazo PrazoEntrega, e.data_entrega DataEntrega, 
      case e.status
        when 0 then 'Não Concluída'
        when 1 then 'Concluída'
      end StatusEntrega,
      ender.nome Endereco
from Pedidos p, Pessoas cli, Pessoas col, Produtos pr, Itens_Pedidos ip, 
    Pagamentos pag, Entregas e, Enderecos ender
where p.id_cliente = cli.id_pessoa and
      p.id_colaborador = col.id_pessoa and
      p.id_pedido = ip.id_pedido and 
      ip.id_produto = pr.id_produto and 
      p.id_pedido = pag.id_pedido and 
      p.id_pedido = e.id_pedido and 
      e.id_endereco = ender.id_endereco

select * from v_DetalhesPedidos