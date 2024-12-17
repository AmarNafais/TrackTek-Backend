
namespace Data.Entities
{
    public class Material : BaseEntity
    {
        public string Name { get; set; }
        public decimal UnitCost { get; set; }
        public int QuantityInStock { get; set; }
        public string Unit { get; set; }
    }
}
