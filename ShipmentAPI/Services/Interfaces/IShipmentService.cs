using ShipmentAPI.Entities;
using ShipmentAPI.Models;

namespace ShipmentAPI.Services.Interfaces
{
    public interface IShipmentService
    {
        int Add(ShipmentDetailInfo shipmentDetail);
        List<ShipmentDetailInfo> GetShipmentHistoryList();
    }
}
