using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.QueryExtensions.Extensions;

namespace Conditus.DynamoDB.QueryExtensions.Pagination
{
    public static class PaginationTokenConverter
    {
        public const char KEY_SEPARATOR = '.';

        public static string GetToken<TEntity>(Dictionary<string, AttributeValue> lastEvaluatedKey)
            where TEntity : new()
        {
            var entityType = typeof(TEntity);
            var tokenKeys = new string[3];

            foreach (var keyAttributeKeyPair in lastEvaluatedKey)
            {
                var keyProperty = entityType.GetProperty(keyAttributeKeyPair.Key);
                var keyStringValue = GetStringFromKeyAttributeValue(keyAttributeKeyPair.Value);

                if (keyProperty.IsDynamoDBHashKeyProperty())
                    tokenKeys[0] = keyStringValue;
                else if (keyProperty.IsDynamoDBRangeKeyProperty())
                    tokenKeys[1] = keyStringValue;
                else
                    tokenKeys[2] = keyStringValue;
            }

            var providedTokenKeys = tokenKeys.Where(k => k != null);
            var token = string.Join(KEY_SEPARATOR, providedTokenKeys);

            return token.EncodeBase64();
        }

        private static string GetStringFromKeyAttributeValue(AttributeValue attributeValue)
        {
            if (attributeValue.S != null)
                return attributeValue.S;

            if (attributeValue.N != null)
                return attributeValue.N;

            throw new ArgumentException("Key attributes can only be string (S) or numeric (N)", nameof(attributeValue));
        }

        public static Dictionary<string, AttributeValue> GetLastEvaluatedKeyFromToken<TEntity>(string base64Token)
        {
            var token = base64Token.DecodeBase64();
            var tokenKeys = token.Split(KEY_SEPARATOR);

            if (tokenKeys.Length >= 3)
                throw new ArgumentException(
                    $"Tokens with local secondary indexes needs to be converted using the {nameof(GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex)} function",
                    nameof(token));

            var entityType = typeof(TEntity);
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>();

            if (tokenKeys.Length == 2)
                lastEvaluatedKey.Add(
                    entityType.GetDynamoDBRangeKeyName(),
                    GetKeyAttributeValueFromString(tokenKeys[1]));
            
            if (tokenKeys.Length >= 1)
                lastEvaluatedKey.Add(
                    entityType.GetDynamoDBHashKeyName(),
                    GetKeyAttributeValueFromString(tokenKeys[0]));

            return lastEvaluatedKey;
        }

        public static Dictionary<string, AttributeValue> GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex<TEntity>(string base64Token, string indexName)
        {
            var token = base64Token.DecodeBase64();
            var tokenKeys = token.Split(KEY_SEPARATOR);

            if (tokenKeys.Length != 3)
                throw new ArgumentException(
                    "Token didn't contain enough key parts. Should consist of 3."
                        + $"\nUse {nameof(GetLastEvaluatedKeyFromToken)} if you aren't using a local secondary index",
                    nameof(token));

            var entityType = typeof(TEntity);
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>
            {
                {entityType.GetDynamoDBHashKeyName(),
                    GetKeyAttributeValueFromString(tokenKeys[0])},
                {entityType.GetDynamoDBRangeKeyName(),
                    GetKeyAttributeValueFromString(tokenKeys[1])},
                {entityType.GetDynamoDBLocalSecondaryRangeKeyName(indexName),
                    GetKeyAttributeValueFromString(tokenKeys[2])},
            };

            return lastEvaluatedKey;
        }

        private static AttributeValue GetKeyAttributeValueFromString(string keyValue)
        {
            var isNumeric = double.TryParse(keyValue, out double dummy);

            if (isNumeric)
                return new AttributeValue { N = keyValue };

            return new AttributeValue { S = keyValue };
        }
    }
}