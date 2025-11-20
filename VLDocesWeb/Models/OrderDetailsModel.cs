namespace VLDocesWeb.Models;
public class OrderDetailsModel
{
    public int Pedido { get; set; }
    public decimal Total { get; set; } // Use 'decimal' para dinheiro, é mais preciso que 'float'
    public DateTime Data { get; set; }
    public string StatusPedido { get; set; }
    public string? Observacoes { get; set; }
    public int IdCliente { get; set; }
    public string Cliente { get; set; }
    
    // Campos que podem ser nulos (das LEFT JOINs)
    public string? Colaborador { get; set; } 
    public string Produto { get; set; }
    public string Descricao { get; set; }
    public decimal PrecoVendido { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoItem { get; set; } // (preco_vend * quantidade)
    public string FormaPagamento { get; set; }
    public string StatusPagamento { get; set; }
    public DateTime PrazoEntrega { get; set; }
    public DateTime? DataEntrega { get; set; } // Use '?' para datas nulas
    public string StatusEntrega { get; set; }
    public string Endereco { get; set; }
}