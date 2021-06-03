using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.QueryExtensions.Pagination;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.PaginationTests.PaginationTokenConverterTests
{
    public class GetTokenTests
    {
        [Fact]
        public void GetToken_WithStringHashKey_ShouldReturnTokenOnlyConsistingOfTheHashKey()
        {
            //Given
            var hashKey = "hashkey";
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithDynamoDBAttributes.HashKey), new AttributeValue{ S = hashKey}}
            };

            //When
            var token = PaginationTokenConverter.GetToken<ClassWithDynamoDBAttributes>(lastEvaluatedKey);

            //Then
            token.Should().Be(hashKey);
        }

        [Fact]
        public void GetToken_WithNumericHashKey_ShouldReturnTokenOnlyConsistingOfTheHashKey()
        {
            //Given
            var hashKey = "123";
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithDynamoDBAttributes.HashKey), new AttributeValue{ N = hashKey}}
            };

            //When
            var token = PaginationTokenConverter.GetToken<ClassWithDynamoDBAttributes>(lastEvaluatedKey);

            //Then
            token.Should().Be(hashKey);
        }

        [Fact]
        public void GetToken_WithHashKeyAndRangeKey_ShouldReturnTokenConsistingOfBothHashAndRangeKey()
        {
            //Given
            var hashKey = "hashKey";
            var rangeKey = "rangeKey";
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithDynamoDBAttributes.HashKey), new AttributeValue{ S = hashKey}},
                {nameof(ClassWithDynamoDBAttributes.RangeKey), new AttributeValue{ S = rangeKey}}
            };

            //When
            var token = PaginationTokenConverter.GetToken<ClassWithDynamoDBAttributes>(lastEvaluatedKey);

            //Then
            var expectedToken = hashKey + PaginationTokenConverter.KEY_SEPARATOR + rangeKey;
            token.Should().Be(expectedToken);
        }

        [Fact]
        public void GetToken_WithRandomOrderOfHashKeyAndRangeKey_ShouldReturnTokenConsistingOfBothHashAndRangeKeyInTheOrderHashThenRange()
        {
            //Given
            var hashKey = "hashKey";
            var rangeKey = "rangeKey";
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithDynamoDBAttributes.RangeKey), new AttributeValue{ S = rangeKey}},
                {nameof(ClassWithDynamoDBAttributes.HashKey), new AttributeValue{ S = hashKey}}
            };

            //When
            var token = PaginationTokenConverter.GetToken<ClassWithDynamoDBAttributes>(lastEvaluatedKey);

            //Then
            var expectedToken = hashKey + PaginationTokenConverter.KEY_SEPARATOR + rangeKey;
            token.Should().Be(expectedToken);
        }

        [Fact]
        public void GetToken_WithHashRangeAndSecondaryRangeKey_ShouldReturnTokenConsistingOfHashRangeAndSecondaryRangeKey()
        {
            //Given
            var hashKey = "hashKey";
            var rangeKey = "rangeKey";
            var secondaryRangeKey = "secondaryRangeKey";
            var lastEvaluatedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithDynamoDBAttributes.HashKey), new AttributeValue{ S = hashKey}},
                {nameof(ClassWithDynamoDBAttributes.RangeKey), new AttributeValue{ S = rangeKey}},
                {nameof(ClassWithDynamoDBAttributes.Index1Key), new AttributeValue{ S = secondaryRangeKey}}
            };

            //When
            var token = PaginationTokenConverter.GetToken<ClassWithDynamoDBAttributes>(lastEvaluatedKey);

            //Then
            var expectedToken = hashKey + PaginationTokenConverter.KEY_SEPARATOR + rangeKey + PaginationTokenConverter.KEY_SEPARATOR + secondaryRangeKey;
            token.Should().Be(expectedToken);
        }
    }
}