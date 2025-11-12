use db_Vilela_Doces
go

alter view v_DetalhesPedidos
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
from Pedidos p
inner join Pessoas cli on p.id_cliente = cli.id_pessoa
left join Pessoas col on p.id_colaborador = col.id_pessoa
inner join Itens_Pedidos ip on p.id_pedido = ip.id_pedido
inner join Produtos pr on ip.id_produto = pr.id_produto
left join Pagamentos pag on p.id_pedido = pag.id_pedido
left join Entregas e on p.id_pedido = e.id_pedido
left join Enderecos ender on e.id_endereco = ender.id_endereco
go