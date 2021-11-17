namespace ProductAssignment.WebApi.Dtos
{
    public class PutProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
    }
}