using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Mappers;

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
                throw new ArgumentException($"{entityType.FullName} doesn't have a DynamoDBTableAttribute", nameof(entityType));

            return dynamoDBTableAttribute.TableName;
        }

        public static Dictionary<string, AttributeValue> GetDynamoDBKey<T>(this T entity)
        {
            var hashKeyProperty = GetDynamoDBHashKeyProperty<T>(entity);
            PropertyInfo rangeKeyProperty = null;
            try
            {
                rangeKeyProperty = GetDynamoDBRangeKeyProperty<T>(entity);
            }
            catch (ArgumentException)
            { }

            var key = new Dictionary<string, AttributeValue>();
            var hashAttributeValue = hashKeyProperty.GetValue(entity)
                .GetAttributeValue(hashKeyProperty.PropertyType);
            key.Add(hashKeyProperty.Name, hashAttributeValue);

            if (rangeKeyProperty == null)
                return key;

            var rangeAttributeValue = rangeKeyProperty.GetValue(entity)
                .GetAttributeValue(rangeKeyProperty.PropertyType);
            key.Add(rangeKeyProperty.Name, rangeAttributeValue);

            return key;
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

        /// <summary>
        /// Why:
        ///   - Is there two checks for the DynamoDBHashKeyAttribute in the Where?
        ///     C# ignores the inherit flag in the GetCustomAttribute method for properties and events.
        ///     As DynamoDBGlobalSecondaryIndexHashKey is inheriting from DynamoDBHashKey,
        ///     GetCustomAttribute(typeof(DynamoDBHashKey), false) will throw an AmbiguousMatchException,
        ///     because it findes both the GSIHashKey and the HashKey attributes.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static PropertyInfo GetDynamoDBHashKeyProperty(this Type entityType)
        {
            var hashProperty = entityType.GetProperties()
                .Where(p =>
                {
                    var propertyRangeAttributes = p.GetCustomAttributes(typeof(DynamoDBHashKeyAttribute));

                    return propertyRangeAttributes.Count() > 0
                        && propertyRangeAttributes.Where(a => (Type)a.TypeId == typeof(DynamoDBHashKeyAttribute)).Count() > 0;
                })
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

        /// <summary>
        /// Why:
        ///   - Is there two checks for the DynamoDBRangeKeyAttribute in the Where?
        ///     C# ignores the inherit flag in the GetCustomAttribute method for properties and events.
        ///     As DynamoDBGlobalSecondaryIndexRangeKey is inheriting from DynamoDBRangeKey,
        ///     GetCustomAttribute(typeof(DynamoDBRangeKey), false) will throw an AmbiguousMatchException,
        ///     because it findes both the GSIRangeKey and the RangeKey attributes.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static PropertyInfo GetDynamoDBRangeKeyProperty(this Type entityType)
        {
            var rangeProperty = entityType.GetProperties()
                .Where(p =>
                {
                    var propertyRangeAttributes = p.GetCustomAttributes(typeof(DynamoDBRangeKeyAttribute));

                    return propertyRangeAttributes.Count() > 0
                        && propertyRangeAttributes.Where(a => (Type)a.TypeId == typeof(DynamoDBRangeKeyAttribute)).Count() > 0;
                })
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
                .Where(p =>
                {
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