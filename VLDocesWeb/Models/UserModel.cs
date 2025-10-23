namespace VLDocesWeb.Models;

public abstract class User
{
    public int Id { get; set; }              // Identificador do usuário
    public string Nome { get; set; }         // Nome completo
    public string Email { get; set; }        // Email de login
    public string Senha { get; set; }        // Senha de acesso
    public string Telefone { get; set; }     // Telefone de contato
    
}