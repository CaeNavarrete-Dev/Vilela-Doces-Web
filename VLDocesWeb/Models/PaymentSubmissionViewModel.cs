namespace VLDocesWeb.Models;

public class PaymentSubmissionViewModel
{
    public int FormaPagamento { get; set; } 
    public decimal? ValorTroco { get; set; } 
    public bool NaoPrecisoTroco { get; set; } 
    public decimal TotalGeral { get; set; }
    public string? Observacoes { get; set; }
}