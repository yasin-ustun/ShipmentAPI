using ShipmentAPI.Entities;
using ShipmentAPI.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ShipmentAPI.Repositories
{
    public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
    {
        public ShipmentRepository(SqlConnection connection, IDbTransaction dbTransaction) : base(connection, dbTransaction )
        {
        }
    }
}
