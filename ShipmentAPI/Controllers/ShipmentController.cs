using Microsoft.AspNetCore.Mvc;
using ShipmentAPI.Entities;
using ShipmentAPI.Models;
using ShipmentAPI.Services;
using ShipmentAPI.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace ShipmentAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShipmentController : ControllerBase
    {
        private int _MaxCapacity;
        private readonly IUnitOfService _UnitOfService;

        public ShipmentController(IUnitOfService unitOfService)
        {
            this._UnitOfService = unitOfService;
        }

        [HttpPost]
        public ResultMessage InsertPackage(PackageRequest request)
        {
            var result = new ResultMessage();

            try
            {
                var packageId = this._UnitOfService.PackageService.SavePackage(request);

                if (packageId <= 0)
                {
                    throw new Exception("Paket bilgileri envantere kaydedilirken bir hata alındı. Lütfen daha sonra tekrar deneyiniz?");
                }
            }
            catch (Exception ex)
            {
                result.SetExceptionMessage(-1, ex.Message);
            }

            return result;
        }

        [HttpGet]
        public ShipmentResponse GetOptimalShipment()
        {
            var response = new ShipmentResponse();

            try
            {
                response.ShipmentDetail = GetOptimalShipmentDetailInfo();
                this._UnitOfService.SaveChangesOfShipment(response.ShipmentDetail);
            }
            catch (Exception ex)
            {
                response.ShipmentDetail = null;
                response.ResultMessage.SetExceptionMessage(-1, ex.Message);
            }

            return response;
        }

        private ShipmentDetailInfo GetOptimalShipmentDetailInfo()
        {
            this._MaxCapacity = this._UnitOfService.Configuration.GetValue<int>("Values:MaxCapacity");

            if (this._MaxCapacity <= 0)
            {
                return null;
            }

            var shipment = new ShipmentDetailInfo();
            shipment.Packages.AddRange(GetPackageListForOptimalShipment());
            shipment.TotalWeight = shipment.Packages.Sum(p => p.Weight * p.Count);
            shipment.TotalAmount = shipment.Packages.Sum(p=>p.Amount * p.Count);
            shipment.ShipmentDate = DateTime.Now;
            return shipment;
        }

        private List<PackageInfo> GetPackageListForOptimalShipment()
        {
            var packages = new List<PackageInfo>();
            var packageList = this._UnitOfService.PackageService.GetAllExistingPackages();
            decimal totalWeight = 0;

            foreach (var package in packageList)
            {
                var weight = package.Weight;
                var count = package.Count;
                var packageWeight = CalculateTotalWeightOfPackage(totalWeight, weight, ref count);

                if (((totalWeight + packageWeight) <= this._MaxCapacity) && (packageWeight > 0))
                {
                    totalWeight += packageWeight;
                    packages.Add(new PackageInfo
                    {
                        Weight = weight,
                        Count = count,
                        Amount = package.Amount
                    });
                }
                else
                {
                    continue;
                }
            }


            return packages;
        }

        private decimal CalculateTotalWeightOfPackage(decimal totalWeight, decimal weight, ref int count)
        {
            decimal packageWeight = weight * count;

            if (packageWeight < this._MaxCapacity)
            {
                if ((totalWeight + packageWeight) <= this._MaxCapacity)
                {
                    return packageWeight;
                }
            }

            count--;
            return CalculateTotalWeightOfPackage(totalWeight, weight, ref count);
        }

        [HttpGet]
        public ShipmentHistoryRepsonse GetPreviousShipment()
        {
            var response = new ShipmentHistoryRepsonse();

            try
            {
                response.Shipments.AddRange(this._UnitOfService.ShipmentService.GetShipmentHistoryList());
            }
            catch (Exception ex)
            {
                response.Shipments = null;
                response.ResultMessage.SetExceptionMessage(-1, ex.Message);
            }

            return response;
        }
    }
}
