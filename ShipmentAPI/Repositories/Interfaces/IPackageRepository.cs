using ShipmentAPI.Entities;

namespace ShipmentAPI.Repositories.Interfaces
{
    public interface IPackageRepository : IRepository<Package>
    {
        int SavePackage(Package package);
        List<Package> GetAllExistingPackages();
        Package GetPackageByWeightAndAmount(decimal weight, decimal amount);
        void UpdatePackage(Package package);
        List<Package> GetPackageByShipmentId(int shipmentId);
    }
}
