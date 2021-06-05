using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.Pagination;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.PaginationTests.PaginationTokenConverterTests
{
    public class GetLastEvaluatedKeyFromTokenTests
    {
        [Fact]
        public void GetLastEvaluatedKeyFromToken_WithStringHashKey_ShouldReturnAttributeValueDictionaryWithHashKeyAttribute()
        {
            //Given
            var hashKeyValue = "hashKeyValue";
            var token = hashKeyValue;
            var base64Token = token.EncodeBase64();

            //When
            var lastEvaluatedKey = PaginationTokenConverter.GetLastEvaluatedKeyFromToken<ClassWithDynamoDBAttributes>(base64Token);

            //Then
            lastEvaluatedKey.Should().HaveCount(1)
                .And.ContainKey(nameof(ClassWithDynamoDBAttributes.HashKey));

            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.HashKey), out AttributeValue hashKeyAttributeValue);
            hashKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { S = hashKeyValue });
        }

        [Fact]
        public void GetLastEvaluatedKeyFromToken_WithNumericHashKey_ShouldReturnAttributeValueDictionaryWithHashKeyAttribute()
        {
            //Given
            var hashKeyValue = "123";
            var token = hashKeyValue;
            var base64Token = token.EncodeBase64();

            //When
            var lastEvaluatedKey = PaginationTokenConverter.GetLastEvaluatedKeyFromToken<ClassWithDynamoDBAttributes>(base64Token);

            //Then
            lastEvaluatedKey.Should().HaveCount(1)
                .And.ContainKey(nameof(ClassWithDynamoDBAttributes.HashKey));
            
            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.HashKey), out AttributeValue hashKeyAttributeValue);
            hashKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { N = hashKeyValue });
        }

        [Fact]
        public void GetLastEvaluatedKeyFromToken_WithHashAndRangeKey_ShouldReturnAttributeValueDictionaryWithHashAndRangeKeyAttributeValue()
        {
            //Given
            var hashKeyValue = "hashKeyValue";
            var rangeKeyValue = "rangeKeyValue";
            var token = hashKeyValue + PaginationTokenConverter.KEY_SEPARATOR + rangeKeyValue;
            var base64Token = token.EncodeBase64();

            //When
            var lastEvaluatedKey = PaginationTokenConverter.GetLastEvaluatedKeyFromToken<ClassWithDynamoDBAttributes>(base64Token);

            //Then
            lastEvaluatedKey.Should().HaveCount(2)
                .And.ContainKey(nameof(ClassWithDynamoDBAttributes.HashKey));

            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.HashKey), out AttributeValue hashKeyAttributeValue);
            hashKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { S = hashKeyValue });

            lastEvaluatedKey.Should()
                .ContainKey(nameof(ClassWithDynamoDBAttributes.RangeKey));

            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.RangeKey), out AttributeValue rangeKeyAttributeValue);
            rangeKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { S = rangeKeyValue });
        }

        [Fact]
        public void GetLastEvaluatedKeyFromToken_WithHashRangeAndSecondaryRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var hashKeyValue = "hashKeyValue";
            var rangeKeyValue = "rangeKeyValue";
            var secondaryKeyValue = "secondaryKeyValue";
            var token = hashKeyValue + PaginationTokenConverter.KEY_SEPARATOR + rangeKeyValue + PaginationTokenConverter.KEY_SEPARATOR + secondaryKeyValue;
            var base64Token = token.EncodeBase64();

            //When
            try
            {
                var lastEvaluatedKey = PaginationTokenConverter.GetLastEvaluatedKeyFromToken<ClassWithDynamoDBAttributes>(base64Token);
                throw new Exception($"{nameof(PaginationTokenConverter.GetLastEvaluatedKeyFromToken)} was expected to throw ArgumentException, but ran without exceptions");
            }
            //Then
            catch (ArgumentException)
            { }
        }
    }
}