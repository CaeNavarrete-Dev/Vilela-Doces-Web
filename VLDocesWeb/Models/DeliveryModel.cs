namespace VLDocesWeb.Models;

public class Delivery {
  public int DeliveryId { get; set; }
  public DateTime DataPrazo { get; set; }
  public DateTime DataEntrega { get; set; }
  public int Status { get; set; }
  public string Observacao { get; set; }
  public int PedidoId { get; set; }
  public int EnderecoId { get; set; }
}