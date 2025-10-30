use VLDocesWebTeste
go


--Procedures--

--Registra cliente--
create procedure p_RegistraCliente @nome varchar(50), @telefone varchar(11), @email varchar(50),  @senha varchar(30), @cpf varchar(11)
as
begin
	SET NOCOUNT ON;
	begin TRY
	if exists (select 1 from Pessoas where email = @email)
		begin
			raiserror('Email informado já está cadastrado', 16,1)
			return;
		end
	if exists (select 1 from Clientes where CPF = @cpf)
		begin
			raiserror('CPF informado já está cadastrado', 16, 1)
			return;
		end
	begin transaction
		insert into Pessoas (nome, telefone, email, senha, tipo_pessoa) values (@nome, @telefone, @email, @senha, 1)
		declare @nova_idPessoa int
		set @nova_idPessoa = SCOPE_IDENTITY()
		insert into Clientes (id_pessoa, CPF) values (@nova_idPessoa, @cpf)	
		commit transaction
	end TRY
	begin catch
		if (@@TRANCOUNT > 0)
			rollback transaction
	end catch
end
go


