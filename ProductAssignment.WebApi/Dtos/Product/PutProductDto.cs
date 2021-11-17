namespace ProductAssignment.WebApi.Dtos.Product
{
    public class PutProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
    }
}