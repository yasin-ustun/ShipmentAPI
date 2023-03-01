using ShipmentAPI.Entities;
using ShipmentAPI.Models;
using ShipmentAPI.Repositories;
using ShipmentAPI.Repositories.Interfaces;
using ShipmentAPI.Services.Interfaces;
using System.Collections.Generic;

namespace ShipmentAPI.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            this._UnitOfWork = unitOfWork;
        }

        public int SavePackage(PackageRequest request)
        {
            var packageId = 0;

            using (var unitWork = this._UnitOfWork)
            {
                var packageEntity = new Package
                {
                    Weight = request.Weight,
                    Amount = request.Amount,
                    CreateDate = DateTime.Now,
                    UnitAmount = request.Amount / request.Weight,
                    ModifyDate = DateTime.Now
                };

                packageId = unitWork.PackageRepository.SavePackage(packageEntity);

                if (packageId <= 0)
                {
                    throw new Exception("Paket bilgileri envantere kaydedilirken bir hata alındı. Lütfen daha sonra tekrar deneyiniz?");
                }
            }
            
            return packageId;
        }
        public List<Package> GetAllExistingPackages()
        {
            using (var unitWork = this._UnitOfWork)
            {
                return this._UnitOfWork.PackageRepository.GetAllExistingPackages();
            }
        }

        public Package GetPackageByWeightAndAmount(decimal weight, decimal amount)
        {
            using (var unitWork = this._UnitOfWork)
            {
                return this._UnitOfWork.PackageRepository.GetPackageByWeightAndAmount(weight, amount);
            }
        }

        public void UpdatePackage(Package package)
        {
            using (var unitWork = this._UnitOfWork)
            {
                this._UnitOfWork.PackageRepository.UpdatePackage(package);
            }
        }
    }
}
