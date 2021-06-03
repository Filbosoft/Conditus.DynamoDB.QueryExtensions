using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Conditus.DynamoDB.MappingExtensions.Mappers;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.IAmazonDynamoDBExtensionTests
{
    public class LoadByLocalSecondaryIndexAsyncTests
    {
        [Fact]
        public async void LoadByLocalSecondaryIndexAsync_WithValidArgumentsAndType_ShouldReturnAnInstanceOfTheType()
        {
            //Given
            var mockDynamoDB = Substitute.For<IAmazonDynamoDB>();
            var mockEntity = new ClassWithDynamoDBAttributes();
            var mockDynamoDBResponse = new QueryResponse
            {
                Items = new List<Dictionary<string, AttributeValue>>
                {
                    mockEntity.GetAttributeValueMap()
                }
            };
            mockDynamoDB.QueryAsync(Arg.Any<QueryRequest>())
                .Returns(Task.FromResult(mockDynamoDBResponse));

            AttributeValue hashValue = new AttributeValue{S = "hashValue"},
                localSecondaryRangeValue = new AttributeValue{S = "secondaryValue"};

            //When
            var resultEntity = await mockDynamoDB.LoadByLocalSecondaryIndexAsync<ClassWithDynamoDBAttributes>(
                hashValue,
                localSecondaryRangeValue,
                ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1);

            //Then
            resultEntity.Should().BeEquivalentTo(mockEntity);
        }

        [Fact]
        public void LoadByLocalSecondaryIndexAsync_WithTypeWithoutHashKey_ShouldThrowArgumentException()
        {
            //Given
            var mockDynamoDB = Substitute.For<IAmazonDynamoDB>();

            AttributeValue hashValue = new AttributeValue{S = "hashValue"},
                localSecondaryRangeValue = new AttributeValue{S = "secondaryValue"};

            //When
            var action = mockDynamoDB.Invoking(e => e.LoadByLocalSecondaryIndexAsync<ClassWithoutDynamoDBAttributes>(
                hashValue,
                localSecondaryRangeValue,
                ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1));

            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}