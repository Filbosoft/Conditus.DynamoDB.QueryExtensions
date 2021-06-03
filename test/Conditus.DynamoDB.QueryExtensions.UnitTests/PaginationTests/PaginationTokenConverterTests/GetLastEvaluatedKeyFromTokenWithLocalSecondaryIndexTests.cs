using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.QueryExtensions.Pagination;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.PaginationTests.PaginationTokenConverterTests
{
    public class GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndexTests
    {

        [Fact]
        public void GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex_WithHashRangeAndSecondaryRangeKey_ShouldReturnAttributeValueMapWithHashRangeAndLocalSecondaryIndexAttributeValues()
        {
            //Given
            var hashKeyValue = "hashKeyValue";
            var rangeKeyValue = "rangeKeyValue";
            var secondaryKeyValue = "localSecondaryRangeKeyValue";
            var token = hashKeyValue + PaginationTokenConverter.KEY_SEPARATOR + rangeKeyValue + PaginationTokenConverter.KEY_SEPARATOR + secondaryKeyValue;

            //When
            var lastEvaluatedKey = PaginationTokenConverter.GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex<ClassWithDynamoDBAttributes>(
                token, ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1);
            //Then
            
            lastEvaluatedKey.Should().HaveCount(3)
                .And.ContainKey(nameof(ClassWithDynamoDBAttributes.HashKey));

            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.HashKey), out AttributeValue hashKeyAttributeValue);
            hashKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { S = hashKeyValue });

            lastEvaluatedKey.Should()
                .ContainKey(nameof(ClassWithDynamoDBAttributes.RangeKey));

            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.RangeKey), out AttributeValue rangeKeyAttributeValue);
            rangeKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { S = rangeKeyValue });

            lastEvaluatedKey.Should()
                .ContainKey(nameof(ClassWithDynamoDBAttributes.Index1Key));

            lastEvaluatedKey.TryGetValue(nameof(ClassWithDynamoDBAttributes.Index1Key), out AttributeValue localSecondaryRangeKeyAttributeValue);
            localSecondaryRangeKeyAttributeValue.Should().BeEquivalentTo(new AttributeValue { S = secondaryKeyValue });
        }

        [Theory]
        [MemberData(nameof(TokensWithoutLocalSecondaryIndex))]
        public void GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex_WithTokenKeyPartsNotBeingThree_ShouldThrowArgumentException(string token)
        {
            //When
            try
            {
                PaginationTokenConverter.GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex<ClassWithDynamoDBAttributes>(
                    token, ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1);

                throw new Exception($"{nameof(PaginationTokenConverter.GetLastEvaluatedKeyFromTokenWithLocalSecondaryIndex)} was expected to throw ArgumentException, but ran without exceptions");
            }
            //Then
            catch (ArgumentException)
            {}
        }

        public static IEnumerable<object[]> TokensWithoutLocalSecondaryIndex
        {
            get
            {
                yield return new Object[] { "hashKey" };
                yield return new Object[] { "hashKey" + PaginationTokenConverter.KEY_SEPARATOR + "rangeKey" };
            }
        }

    }
}