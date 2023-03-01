using ShipmentAPI.Entities;
using ShipmentAPI.Models;
using ShipmentAPI.Repositories.Interfaces;
using ShipmentAPI.Services.Interfaces;

namespace ShipmentAPI.Services
{
    public class UnitOfService : IUnitOfService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private IPackageService _PackageService;
        private IShipmentService _ShipmentService;

        public UnitOfService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this._UnitOfWork = unitOfWork;
            this.Configuration = configuration;
        }

        public IPackageService PackageService => _PackageService ?? new PackageService(this._UnitOfWork);

        public IShipmentService ShipmentService => _ShipmentService ?? new ShipmentService(this._UnitOfWork);

        public IConfiguration Configuration { get; set; }

        public void SaveChangesOfShipment(ShipmentDetailInfo shipmentInfo)
        {
            using (var unitWork = this._UnitOfWork)
            {
                var shipment = new Shipment
                {
                    TotalWeight = shipmentInfo.TotalWeight,
                    TotalAmount = shipmentInfo.TotalAmount,
                    ShipmentDate = DateTime.Now,
                    CreateDate = DateTime.Now
                };

                var shipmentId = unitWork.ShipmentRepository.Add(shipment);

                foreach (var packageInfo in shipmentInfo.Packages)
                {
                    var package = unitWork.PackageRepository.GetPackageByWeightAndAmount(packageInfo.Weight, packageInfo.Amount);

                    if (package != null)
                    {
                        var shipmentDetail = new ShipmentDetail
                        {
                            ShipmentId = shipmentId,
                            PackageId = package.Id,
                            PackageCount = packageInfo.Count,
                            CreateDate = DateTime.Now
                        };

                        unitWork.ShipmentDetailRepository.Add(shipmentDetail);
                        package.Count -= packageInfo.Count;
                        package.ModifyDate = DateTime.Now;
                        unitWork.PackageRepository.UpdatePackage(package);
                    }
                }
            }
        }
    }
}
