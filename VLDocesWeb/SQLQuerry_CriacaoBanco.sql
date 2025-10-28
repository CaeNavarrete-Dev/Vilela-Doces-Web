-----Cria��o do Banco-----
create database db_Vilela_Doces

use db_Vilela_Doces
go

-----Cria��o das Tabelas-----

-----//Pessoas//
create table Pessoas
(
	id_pessoa		int			not null	primary key		identity,
	nome			varchar(50) not null,
	telefone		varchar(11) not null,
	email			varchar(50) not null,
	senha			varchar(30) not null
)
go

----//Clientes//
create table Clientes
(
	id_pessoa	int	not null primary key references Pessoas (id_pessoa),
	cpf			varchar(11)	not null,
)
go

----//Endere�os//
create table Enderecos
(
	id_endereco int not null primary key identity,
	rua varchar(100),
	numero varchar(10),
	bairro varchar(50),
	cep varchar(9) not null,
	complemento varchar(50),
	-- chave estrangeira
	id_cliente int not null references Clientes (id_pessoa)
)
go

----//Colaboradores//
create table Colaboradores
(
	id_pessoa int  primary key references Pessoas (id_pessoa),
	observacao varchar(500)
)
go

----//Pedidos//
create table Pedidos
(
	id_pedido int not null primary key identity,
	total money null,
	data_hora datetime not null,
	status int not null, check(status between 0 and 5), 
	--chaves estrangeiras
	id_cliente int not null references Clientes (id_pessoa),
	id_colaborador int not null references Colaboradores (id_pessoa)
	-- Status:
	-- 0.Em an�lise 
	-- 1.Confirmado 
	-- 2.Cancelado 
	-- 3.Pronto p/ retirada 
	-- 4.Negado 
	-- 5.Finalizado
)
go

----//Pagamentos//
create table Pagamentos
(
	id_pagamento int not null primary key identity,
	forma_pagamento int not null, check (status between 0 and 3), 
	status int not null, check(status between 0 and 3), 
	data_pagamento datetime not null,
	data_limite datetime not null,
	valor_pago money not null,
	-- chave estrangeira
	id_pedido int not null references Pedidos (id_pagamento),
	id_cliente int not null references Clientes (id_pessoa)
-- Forma Pag
-- 0.Dinheiro
-- 1.Pix
-- 2.Cart�o Cr�dito
-- 3.Cart�o D�bito
-- Status
-- 0. Pago
-- 2. Cancelado
-- 3. Estornado
)
go

----//Entregas//
create table Entregas
(
	id_entrega int not null primary key identity,
	data_prazo datetime not null,
	data_entrega datetime not null,
	status int not null, check(status between 0 and 1), 
	observacao varchar(500),
	-- chave estrangeira
	id_pedidos int not null references Pedidos (id_pedido),
	id_cliente int not null references Clientes (id_pessoa)
-- Status
-- 0. Concluido
-- 1. N�o concluido
)
go

----//Categorias//
create table Categorias
(
	id_categoria int not null primary key identity,
	nome varchar(50),
	descricao varchar(100) 
)
go

----//Produtos//
create table Produtos
(
	id_produto int not null primary key identity,
	nome_produto varchar(50) not null,
	preco money not null,
	-- chave estrangeira
	id_categoria int not null references Categorias (id_categoria)
)
go

----//Itens_Pedido//
create table Itens_Pedidos
(
	preco_vend money not null,
	quantidade int not null,
	-- chaves estrangeiras
	id_pedido int not null references Pedidos (id_pedido),
	id_produto int not null references Produtos (id_produto),
	-- chaves primarias
	primary key (id_pedido, id_produto)
)
go
