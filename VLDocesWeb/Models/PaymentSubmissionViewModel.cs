namespace VLDocesWeb.Models;

public class PaymentSubmissionViewModel
{
    public int FormaPagamento { get; set; } 
    public decimal? ValorTroco { get; set; } 
    public bool NaoPrecisoTroco { get; set; } 
    public decimal TotalGeral { get; set; }
    public string? Observacoes { get; set; }
    public DateTime? DataEntregaAgendada { get; set; }
    public int OpcaoPagamentoEncomenda { get; set; }
    // 1 = Total Pix, 2 = 50/50 Pix, 3 = 50 Pix / 50 Dinheiro
}