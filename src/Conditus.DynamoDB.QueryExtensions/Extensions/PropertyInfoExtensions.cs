using System.Reflection;
using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsDynamoDBHashKeyProperty(this PropertyInfo property)
        {
            var hashKeyAttribute = property.GetCustomAttribute(typeof(DynamoDBHashKeyAttribute));

            return hashKeyAttribute != null;
        }

        public static bool IsDynamoDBRangeKeyProperty(this PropertyInfo property)
        {
            var hashKeyAttribute = property.GetCustomAttribute(typeof(DynamoDBRangeKeyAttribute));

            return hashKeyAttribute != null;
        }
    }
}