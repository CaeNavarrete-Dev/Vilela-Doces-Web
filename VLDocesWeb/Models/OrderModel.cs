namespace VLDocesWeb.Models {
    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public  DateTime Data { get; set; }
        public int Status {get; set;}
        public int IdCliente { get; set; }
        public int IdColaborador { get; set; }
    }
}

