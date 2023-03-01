namespace ShipmentAPI.Extensions
{
    public static class Extensions
    {
        public static string GetAllScript(this Type entityType)
        {
            if (entityType == null)
            {
                return string.Empty;
            }

            var sql = string.Format(@"Select * From dbo.{0} As T(Nolock)", entityType.Name);
            return sql;
        }

        public static string InsertScript<TEntity>(this TEntity entity)
        {
            if (entity == null)
            {
                return string.Empty;
            }

            var type = entity.GetType();
            var tableName = type.Name;
            var columns = new List<string>();
            var parameters = new List<string>();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if(property.Name != "Id")
                {
                    columns.Add(property.Name);
                    parameters.Add("@" + property.Name);
                }
            }

            var sql = string.Format(@"Insert Into dbo.{0} ({1}) Values ({2}) Select SCOPE_IDENTITY()", tableName, string.Join(",", columns), string.Join(',', parameters));
            return sql;
        }

        public static Dictionary<string, object> GetInsertParameters<TEntity>(this TEntity entity)
        {
            var parametersDict = new Dictionary<string, object>();

            if (entity == null)
            {
                return parametersDict;
            }

            var type = entity.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if(property.Name != "Id")
                {
                    parametersDict.Add("@" + property.Name, property.GetValue(entity));
                }
            }

            return parametersDict;
        }
    }
}
