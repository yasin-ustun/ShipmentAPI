namespace ShipmentAPI.Entities
{
    public class ShipmentDetail
    {
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public int PackageId { get; set; }
        public int PackageCount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
