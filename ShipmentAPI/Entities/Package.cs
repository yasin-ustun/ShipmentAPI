namespace ShipmentAPI.Entities
{
    public class Package
    {
        public int Id { get; set; }
        public decimal Weight { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitAmount { get; set; }
        public int Count { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
