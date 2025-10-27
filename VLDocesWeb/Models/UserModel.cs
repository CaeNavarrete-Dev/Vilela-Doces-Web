namespace VLDocesWeb.Models;

public abstract class User
{
    public int Id { get; set; }
    public string Nome { get; set; } 
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Telefone { get; set; }
    public int Tipo_Usuario { get; set; }
    
}