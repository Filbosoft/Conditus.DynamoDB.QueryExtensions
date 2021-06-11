using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBKeyTests
    {
        [Fact]
        public void GetDynamoDBKey_WithEntityWithOnlyHashKey_ShouldReturnKeyConsistingOfOnlyTheHashKey()
        {
            //Given
            var entity = new ClassWithHashKey { Id = "ID1" };

            //When
            var key = entity.GetDynamoDBKey();

            //Then
            var expectedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithHashKey.Id), new AttributeValue { S = entity.Id }}
            };
            key.Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedKey);
        }

        [Fact]
        public void GetDynamoDBKey_WithEntityWithHashAndRangeKey_ShouldReturnKeyConsistingOfHashAndRangeKey()
        {
            //Given
            var entity = new ClassWithHashAndRangeKey
            {
                Id = "Id123",
                Name = "RangeKey"
            };

            //When
            var key = entity.GetDynamoDBKey();

            //Then
            var expectedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithHashAndRangeKey.Id), new AttributeValue { S = entity.Id }},
                {nameof(ClassWithHashAndRangeKey.Name), new AttributeValue { S = entity.Name }}
            };
            key.Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedKey);
        }

        [Fact]
        public void GetDynamoDBKey_WithEntityWithHashAndDateTimeRangeKey_ShouldReturnKeyConsistingOfHashAndRangeKey()
        {
            //Given
            var entity = new ClassWithHashAndDateTimeRangeKey
            {
                Id = "Id123",
                CreatedAt = DateTime.UtcNow
            };

            //When
            var key = entity.GetDynamoDBKey();

            //Then
            var expectedKey = new Dictionary<string, AttributeValue>
            {
                {nameof(ClassWithHashAndDateTimeRangeKey.Id), new AttributeValue { S = entity.Id }},
                {nameof(ClassWithHashAndDateTimeRangeKey.CreatedAt), DateTimeMapper.GetAttributeValue(entity.CreatedAt)}
            };
            key.Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedKey);
        }
    }
}
