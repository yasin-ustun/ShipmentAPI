using ShipmentAPI.Entities;
using ShipmentAPI.Models;
using ShipmentAPI.Repositories.Interfaces;
using ShipmentAPI.Services.Interfaces;

namespace ShipmentAPI.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ShipmentService(IUnitOfWork unitOfWork)
        {
            this._UnitOfWork = unitOfWork;
        }

        public int Add(ShipmentDetailInfo shipmentDetail)
        {
            using (var unitWork = this._UnitOfWork)
            {
                var shipment = new Shipment
                {
                    TotalWeight = shipmentDetail.TotalWeight,
                    TotalAmount = shipmentDetail.TotalAmount,
                    ShipmentDate = DateTime.Now,
                    CreateDate = DateTime.Now
                };

                return unitWork.ShipmentRepository.Add(shipment);
            }
        }
        public List<ShipmentDetailInfo> GetShipmentHistoryList()
        {
            var shipmentHistoryList = new List<ShipmentDetailInfo>();

            using (var unitWork = this._UnitOfWork)
            {
                var shipmentList = unitWork.ShipmentRepository.GetAll().OrderBy(s => s.ShipmentDate);

                foreach (var shipment in shipmentList)
                {
                    var packageInfo = unitWork.PackageRepository
                        .GetPackageByShipmentId(shipment.Id)
                        .Select(p => new PackageInfo
                        {
                            Weight = p.Weight,
                            Amount = p.Amount,
                            Count = p.Count
                        }).ToList();

                    var shipmentDetail = new ShipmentDetailInfo();
                    shipmentDetail.Packages.AddRange(packageInfo);
                    shipmentDetail.ShipmentDate = shipment.ShipmentDate;
                    shipmentDetail.TotalWeight = shipment.TotalWeight;
                    shipmentDetail.TotalAmount = shipment.TotalAmount;
                    shipmentHistoryList.Add(shipmentDetail);
                }
            }

            return shipmentHistoryList;
        }
    }
}
