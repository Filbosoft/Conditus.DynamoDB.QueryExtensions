using System;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsDynamoDBHashKeyProperty(this PropertyInfo property)
        {
            var propertyHashAttributes = property.GetCustomAttributes(typeof(DynamoDBHashKeyAttribute));

            return propertyHashAttributes
                .Where(a => (Type)a.TypeId == typeof(DynamoDBHashKeyAttribute))
                .Count() > 0;
        }

        public static bool IsDynamoDBRangeKeyProperty(this PropertyInfo property)
        {
            var propertyRangeAttributes = property.GetCustomAttributes(typeof(DynamoDBRangeKeyAttribute));

            return propertyRangeAttributes
                .Where(a => (Type)a.TypeId == typeof(DynamoDBRangeKeyAttribute))
                .Count() > 0;
        }
    }
}