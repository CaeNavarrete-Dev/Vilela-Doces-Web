-----Criação do Banco-----
create database db_Vilela_Doces
go

use db_Vilela_Doces
go

-----Criação das Tabelas-----

-----//Pessoas//
create table Pessoas
(
	id_pessoa		int			not null	primary key		identity,
	nome			varchar(50) not null,
	telefone		varchar(11) not null,
	email			varchar(50) not null,
	senha			varchar(30) not null,
	tipo_pessoa		int			not	null,		check (tipo_pessoa between 0 and 1) 
	--tipo_pessoa: 0 colaborador, 1 cliente
)
go

----//Clientes//
create table Clientes
(
	id_pessoa	int	not null primary key references Pessoas (id_pessoa),
	cpf			varchar(11)	not null,
)
go

----//Colaboradores//
create table Colaboradores
(
	id_pessoa int  primary key references Pessoas (id_pessoa),
	observacao varchar(500)
)
go

--Colaborador Teste--
insert into Pessoas (nome, telefone, email, senha, tipo_pessoa) values ('adm','17988145012','adm@gmail','123', 0)
insert into Colaboradores values (1,'Adm teste')
insert into Pessoas (nome, telefone, email, senha, tipo_pessoa) values ('caetano','17988145012','caetano@gmail','123', 1)
insert into Clientes values (1,'48148148186')

----//Endereços//
create table Enderecos
(
	id_endereco int not null primary key identity,
	nome varchar(100),
	rua varchar(100),
	numero varchar(10),
	bairro varchar(50),
	cep varchar(9) not null,
	complemento varchar(50),
	cidade varchar(100),
	uf varchar(100),
	-- chave estrangeira
	id_cliente int null references Clientes (id_pessoa)
)
go

---- Endereço da Loja ----
insert into Enderecos(nome, rua, numero, bairro, cep, cidade, uf) 
values('Retirada na Loja', 'Rua dos Bobos', 'nº0', 'Jd. dos Bobos', '12345123', 'Potirendaba', 'SP')
go
insert into Enderecos(nome, rua, numero, bairro, cep, cidade, uf, id_cliente) 
values('Casa', 'São Carlos', '360', 'Jd. Europa', '15014480', 'São José do Rio Preto', 'SP', 2)
GO

----//Categorias//
create table Categorias
(
	id_categoria int not null primary key identity,
	nome varchar(50)
	-- Categoria
	-- 1. Entrega
	-- 2. Pronta Entrega
)
go

--Categorias Teste--
insert into Categorias (nome) values ('Pronta Entrega')
insert into Categorias (nome) values ('Encomenda')

----//Produtos//
create table Produtos
(
	id_produto int not null primary key identity,
	nome_produto varchar(50) not null,
	preco money not null,
	descricao varchar(100)	not	null,
	-- chave estrangeira
	id_categoria int not null references Categorias (id_categoria)
)
go

----//Pedidos//
create table Pedidos
(
	id_pedido int not null primary key identity,
	total money null,
	data_hora datetime not null,
	status int not null, check(status between 0 and 5),
	observacoes varchar(200) default null,
	frete money default 0.00,
	--chaves estrangeiras
	id_cliente int not null references Clientes (id_pessoa),
	id_colaborador int references Colaboradores (id_pessoa)
	-- Status:
	-- 0.Em análise 
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
	data_pagamento datetime,
	data_limite datetime not null,
	valor_pago money not null,
	-- chave estrangeira
	id_pedido int not null references Pedidos(id_pedido)
-- Forma Pag
-- 0.Dinheiro
-- 1.Pix
-- Status
-- 0. Pendente
-- 1. Pago
-- 2. Cancelado
-- 3. Estornado
)
go

----//Entregas//
create table Entregas
(
	id_entrega int not null primary key identity,
	data_prazo datetime not null,
	data_entrega datetime,
	status int not null, check(status between 0 and 1), 
	observacao varchar(500),
	-- chave estrangeira
	id_pedido int not null references Pedidos (id_pedido),
	id_endereco int not null references Enderecos (id_endereco)
-- Status
-- 0. Não Concluída
-- 1. Concluída
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
	-- chaves primárias
	primary key (id_pedido, id_produto)
)
go

select * from Categorias
select * from Clientes
select * from Colaboradores
select * from Enderecos
select * from Pessoas
select * from Entregas
select * from Itens_Pedidos
select * from Pagamentos
select * from Pedidos
select * from Produtos