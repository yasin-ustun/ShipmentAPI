using ShipmentAPI.Models;

namespace ShipmentAPI.Services.Interfaces
{
    public interface IUnitOfService
    {
        IPackageService PackageService { get; }
        IShipmentService ShipmentService { get; }
        IConfiguration Configuration { get; }
        void SaveChangesOfShipment(ShipmentDetailInfo shipmentDetailInfo);
    }
}
