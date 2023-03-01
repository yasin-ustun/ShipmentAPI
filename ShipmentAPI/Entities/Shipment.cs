using System.Security.Principal;

namespace ShipmentAPI.Entities
{
    public class Shipment
    {
        public int Id { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ShipmentDate  { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
