using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
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
                {nameof(ClassWithHashKey.Id), new AttributeValue { S = entity.Id }},
                {nameof(ClassWithHashKey.Name), new AttributeValue { S = entity.Name }}
            };
            key.Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedKey);
        }
    }
}
