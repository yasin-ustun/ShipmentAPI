using System.Data;

namespace ShipmentAPI.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPackageRepository PackageRepository { get; }
        IShipmentRepository ShipmentRepository { get; }
        IShipmentDetailRepository ShipmentDetailRepository { get; }
        void Commit();
    }
}
