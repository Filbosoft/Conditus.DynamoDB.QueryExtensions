using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Mappers;

namespace Conditus.DynamoDB.QueryExtensions.Extensions
{
    public static class IAmazonDynamoDBExtensions
    {
        public async static Task<T> LoadByLocalSecondaryIndexAsync<T>(
            this IAmazonDynamoDB dynamoDB,
            AttributeValue hashValue,
            AttributeValue rangeValue,
            string localSecondaryIndexName
        )
            where T : new()
        {
            var entityType = typeof(T);
            string tableName = EntityDynamoDBExtensions.GetDynamoDBTableName(entityType),
                hashKeyName = EntityDynamoDBExtensions.GetDynamoDBHashKeyName(entityType),
                localSecondaryRangeKeyName = EntityDynamoDBExtensions.GetDynamoDBLocalSecondaryRangeKeyName(entityType, localSecondaryIndexName);

            var query = new QueryRequest
            {
                TableName = tableName,
                Select = "ALL_ATTRIBUTES",
                IndexName = localSecondaryIndexName,
                KeyConditionExpression = $"{hashKeyName} = :v_hash_key AND {localSecondaryRangeKeyName} = :v_range_key",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_hash_key", hashValue},
                    {":v_range_key", rangeValue}
                }
            };

            var response = await dynamoDB.QueryAsync(query);
            var item = response.Items
                .FirstOrDefault();

            if (item == null)
                return default(T);

            var mappedItem = item.ToEntity<T>();
            
            return mappedItem;
        }
    }
}