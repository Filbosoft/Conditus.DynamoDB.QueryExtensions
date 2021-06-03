using System;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.Extensions
{
    public static class EntityDynamoDBExtensions
    {
        public static string GetDynamoDBTableName<T>(this T entity)
        {
            var entityType = entity.GetType();
            
            return GetDynamoDBTableName(entityType);
        }

        public static string GetDynamoDBTableName(this Type entityType)
        {
            var dynamoDBTableAttribute = entityType.GetCustomAttribute(typeof(DynamoDBTableAttribute), true) as DynamoDBTableAttribute;
            
            if (dynamoDBTableAttribute == null)
                throw new ArgumentException($"{entityType.FullName} doesn't have a DynamoDBTableAttribute" , nameof(entityType));
            
            return dynamoDBTableAttribute.TableName;
        }
        
        public static string GetDynamoDBHashKeyName<T>(this T entity)
        {
            var hashProperty = GetDynamoDBHashKeyProperty(entity);
            
            return hashProperty.Name;
        }

        public static PropertyInfo GetDynamoDBHashKeyProperty<T>(this T entity)
        {
            var entityType = entity.GetType();
            
            return GetDynamoDBHashKeyProperty(entityType);
        }

        public static string GetDynamoDBHashKeyName(this Type entityType)
        {
            var hashProperty = GetDynamoDBHashKeyProperty(entityType);
            
            return hashProperty.Name;
        }

        public static PropertyInfo GetDynamoDBHashKeyProperty(this Type entityType)
        {
            var dynamoDBTableAttribute = entityType.GetCustomAttribute(typeof(DynamoDBTableAttribute), true) as DynamoDBTableAttribute;
            
            var hashProperty = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(DynamoDBHashKeyAttribute), false) != null)
                .FirstOrDefault();
            
            if (hashProperty == null)
                throw new ArgumentException($"No DynamoDBHashKey defined on {entityType.FullName}", nameof(entityType));
            
            return hashProperty;
        }

        public static string GetDynamoDBRangeKeyName<T>(this T entity)
        {
            var rangeProperty = GetDynamoDBRangeKeyProperty(entity);
            
            return rangeProperty.Name;
        }

        public static PropertyInfo GetDynamoDBRangeKeyProperty<T>(this T entity)
        {
            var entityType = entity.GetType();
            
            return GetDynamoDBRangeKeyProperty(entityType);
        }

        public static string GetDynamoDBRangeKeyName(this Type entityType)
        {
            var rangeProperty = GetDynamoDBRangeKeyProperty(entityType);
            
            return rangeProperty.Name;
        }

        public static PropertyInfo GetDynamoDBRangeKeyProperty(this Type entityType)
        {
            var rangeProperty = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(DynamoDBRangeKeyAttribute), false) != null)
                .FirstOrDefault();
            
            if (rangeProperty == null)
                throw new ArgumentException($"No DynamoDBRangeKey defined on {entityType.FullName}", nameof(entityType));
            
            return rangeProperty;
        }

        public static string GetDynamoDBLocalSecondaryRangeKeyName<T>(this T entity, string secondaryLocalIndexName)
        {
            var localSecondaryRangeKeyProperty = GetDynamoDBLocalSecondaryRangeKeyProperty(entity, secondaryLocalIndexName);
            
            return localSecondaryRangeKeyProperty.Name;
        }

        public static PropertyInfo GetDynamoDBLocalSecondaryRangeKeyProperty<T>(this T entity, string secondaryLocalIndexName)
        {
            var entityType = entity.GetType();
            
            return GetDynamoDBLocalSecondaryRangeKeyProperty(entityType, secondaryLocalIndexName);
        }

        public static string GetDynamoDBLocalSecondaryRangeKeyName(this Type entityType, string secondaryLocalIndexName)
        {
            var localSecondaryRangeKeyProperty = GetDynamoDBLocalSecondaryRangeKeyProperty(entityType, secondaryLocalIndexName);
            
            return localSecondaryRangeKeyProperty.Name;
        }

        public static PropertyInfo GetDynamoDBLocalSecondaryRangeKeyProperty(this Type entityType, string secondaryLocalIndexName)
        {
            var localSecondaryRangeKeyProperty = entityType.GetProperties()
                .Where(p => {
                    var customAttribute = p.GetCustomAttribute(typeof(DynamoDBLocalSecondaryIndexRangeKeyAttribute), false) as DynamoDBLocalSecondaryIndexRangeKeyAttribute;
                     
                    return customAttribute != null 
                        && customAttribute.IndexNames.Contains(secondaryLocalIndexName);
                })
                .FirstOrDefault();
            
            if (localSecondaryRangeKeyProperty == null)
                throw new ArgumentException(
                    $"No property associated with the secondaryLocalIndexName: {secondaryLocalIndexName} defined on {entityType.FullName}",
                    nameof(entityType));
            
            return localSecondaryRangeKeyProperty;
        }
    }
}