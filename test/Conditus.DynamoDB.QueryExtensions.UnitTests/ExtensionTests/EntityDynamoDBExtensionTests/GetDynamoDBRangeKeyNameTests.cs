using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBRangeKeyNameTests
    {
        [Fact]
        public void GetDynamoDBRangeKeyName_WithEntityWithRangeKey_ShouldReturnTheRangeKeyName()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();

            //When
            var resultRangeKeyName = entity.GetDynamoDBRangeKeyName();

            //Then
            resultRangeKeyName.Should().Be(nameof(entity.RangeKey));
        }

        [Fact]
        public void GetDynamoDBRangeKeyName_WithEntityWithoutRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBRangeKeyName());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBRangeKeyName_WithTypeOfEntityWithRangeKey_ShouldReturnTheRangeKeyName()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);

            //When
            var resultRangeKeyName = entityType.GetDynamoDBRangeKeyName();

            //Then
            resultRangeKeyName.Should().Be(nameof(ClassWithDynamoDBAttributes.RangeKey));
        }

        [Fact]
        public void GetDynamoDBRangeKeyName_WithTypeOfEntityWithoutRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entityType = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entityType.Invoking(e => e.GetDynamoDBRangeKeyName());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
