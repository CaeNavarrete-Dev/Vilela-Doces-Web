create database VLDocesWebTeste
go

use VLDocesWebTeste
go


--Criação Tabelas--

--/Pesssoas/--
create table Pessoas
(
	id_pessoa		int			not	null		primary key		identity,
	nome			varchar(50) not null,
	telefone		varchar(11) not null,
	email			varchar(50) not null,
	senha			varchar(30) not null,
	tipo_pessoa		int			not	null,		check (tipo_pessoa between 0 and 1) --tipo_pessoa: 0 colaborador, 1 cliente
)
go

--/Clientes/--
create table Clientes
(
	id_pessoa		int			not	null		primary	key		references Pessoas (id_pessoa),
	CPF				varchar(11)	not	null,
)
go

--/Enderecos/--


--/Colaboradores--
create table Colaboradores
(
	id_pessoa		int			not	null		primary	key		references	Pessoas	(id_pessoa),
	observacao		varchar(100)
)
go
--Colaborador Teste--
insert into Pessoas (nome,telefone,email,senha,tipo_pessoa) values ('adm','17988145012','adm@gmail','123',0)
insert into Colaboradores values (3,'Adm teste')



--Categorias--
create table Categorias
(
	id_categoria int not null primary key identity,
	nome varchar(50)
	-- Categoria
	-- 1. Entrega
	-- 2. Pronta Entrega
)
go
--Caegorias--
insert into Categorias (nome) values ('Pronta Entrega')
insert into Categorias (nome) values ('Encomenda')

--Produtos--
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

