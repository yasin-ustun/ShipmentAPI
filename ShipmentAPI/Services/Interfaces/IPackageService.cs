using ShipmentAPI.Entities;
using ShipmentAPI.Models;

namespace ShipmentAPI.Services.Interfaces
{
    public interface IPackageService
    {
        int SavePackage(PackageRequest request);
        List<Package> GetAllExistingPackages();
        Package GetPackageByWeightAndAmount(decimal weight, decimal amount);
        void UpdatePackage(Package package);
    }
}
