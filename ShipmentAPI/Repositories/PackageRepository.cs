using Dapper;
using ShipmentAPI.Entities;
using ShipmentAPI.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ShipmentAPI.Repositories
{
    public class PackageRepository : Repository<Package>, IPackageRepository
    {
        public PackageRepository(SqlConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction)
        {
        }

        public int SavePackage(Package package)
        {
            var sql = @"
                If Not Exists(Select 1 From dbo.Package As P(Nolock) Where P.Weight = @Weight And P.Amount = @Amount)
                Begin
	                Insert Into dbo.Package
	                (Weight, Amount, UnitAmount, Count, CreateDate)
	                Values
	                (@Weight, @Amount, @UnitAmount, 1, @CreateDate)

                    Select SCOPE_IDENTITY()
                End
                Else
                Begin
	                Update P
		                Set P.Count = P.Count + 1,
			                P.ModifyDate = @ModifyDate
	                From dbo.Package As P(Nolock) 
	                Where 
		                P.Weight = @Weight 
	                And P.Amount = @Amount

                    Select Id From dbo.Package As P(Nolock) Where P.Weight = @Weight And P.Amount = @Amount
                End";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@Weight", package.Weight);
            parameters.Add("@Amount", package.Amount);
            parameters.Add("@UnitAmount", package.UnitAmount);
            parameters.Add("@CreateDate", package.CreateDate);
            parameters.Add("@ModifyDate", package.ModifyDate);
            var param = new DynamicParameters(parameters);
            var packageId = this.Connection.ExecuteScalar<int>(sql, param, transaction: this.DbTransaction);

            return packageId;
        }

        public List<Package> GetAllExistingPackages()
        {
            var sql = @"
                Select
	                Id,
	                Weight,
	                Amount,
	                UnitAmount,
	                Count,
	                CreateDate,
	                ModifyDate
                From dbo.Package As P(Nolock)
                Where Count > 0
                Order By P.UnitAmount Desc";

            var list = this.Connection.Query<Package>(sql, transaction : this.DbTransaction).ToList();

            return list;
        }

        public Package GetPackageByWeightAndAmount(decimal weight, decimal amount)
        {
            var sql = @"
                Select
	                Id,
	                Weight,
	                Amount,
	                UnitAmount,
	                Count,
	                CreateDate,
	                ModifyDate
                From dbo.Package As P(Nolock)
                Where 
	                Weight = @Weight
                And Amount = @Amount";

            var parametersDict = new Dictionary<string, object>();
            parametersDict.Add("@Weight", weight);
            parametersDict.Add("@Amount", amount);
            var parameters = new DynamicParameters(parametersDict);
            var package = this.Connection.QuerySingleOrDefault<Package>(sql, parameters, transaction: this.DbTransaction);
            return package;
        }

        public void UpdatePackage(Package package)
        {
            var sql = @"
                Update P
	            Set Count = @Count,
	                ModifyDate = @ModifyDate
                From dbo.Package As P(Nolock)
                Where 
	                Id = @Id";

            var parametersDict = new Dictionary<string, object>();
            parametersDict.Add("@Count", package.Count);
            parametersDict.Add("@ModifyDate", package.ModifyDate);
            parametersDict.Add("@Id", package.Id);
            var parameters = new DynamicParameters(parametersDict);
            this.Connection.QuerySingleOrDefault<Package>(sql, parameters, transaction: this.DbTransaction);
        }

        public List<Package> GetPackageByShipmentId(int shipmentId)
        {
            var sql = @"
                Select 
	                P.Weight,
	                P.Amount,
	                D.PackageCount As Count
                From dbo.ShipmentDetail As D(Nolock)
                Inner Join dbo.Package As P(Nolock) On
	                D.PackageId = P.Id
                Where D.ShipmentId = @ShipmentId";

            var parametersDict = new Dictionary<string, object>();
            parametersDict.Add("@ShipmentId", shipmentId);
            var parameters = new DynamicParameters(parametersDict);
            return this.Connection.Query<Package>(sql, parameters, transaction: this.DbTransaction).ToList();
        }
    }
}
