create database VLDocesWebTeste
go

use VLDocesWebTeste
go

--Cria��o Tabelas--

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

select * from Pessoas
go

--/Clientes/--
create table Clientes
(
	id_pessoa		int			not	null		primary	key		references Pessoas (id_pessoa),
	CPF				varchar(11)	not	null,
)
go

--/Enderecos/--
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
	id_cliente int not null references Clientes (id_pessoa)
)
go

select * from Enderecos
go

drop table Enderecos

--/Colaboradores--
create table Colaboradores
(
	id_pessoa		int			not	null		primary	key		references	Pessoas	(id_pessoa),
	observacao		varchar(100)
)
go