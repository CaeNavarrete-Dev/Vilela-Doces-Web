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