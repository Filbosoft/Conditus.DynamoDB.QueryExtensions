using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBLocalSecondaryRangeKeyNameTests
    {
        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyName_WithEntityWithLocalSecondaryRangeKey_ShouldReturnTheLocalSecondaryRangeKeyName()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();

            //When
            var resultRangeKeyName = entity.GetDynamoDBLocalSecondaryRangeKeyName(TestEntitySecondaryLocalIndexes.Index1);

            //Then
            resultRangeKeyName.Should().Be(nameof(entity.Index1Key));
        }

        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyName_WithEntityWithoutLocalSecondaryRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBLocalSecondaryRangeKeyName("NonExistingIndex"));
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyName_WithTypeOfEntityWithLocalSecondaryRangeKey_ShouldReturnTheLocalSecondaryRangeKeyName()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);

            //When
            var resultRangeKeyName = entityType.GetDynamoDBLocalSecondaryRangeKeyName(TestEntitySecondaryLocalIndexes.Index1);

            //Then
            resultRangeKeyName.Should().Be(nameof(ClassWithDynamoDBAttributes.Index1Key));
        }

        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyName_WithTypeOfEntityWithoutLocalSecondaryRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entityType = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entityType.Invoking(e => e.GetDynamoDBLocalSecondaryRangeKeyName("NonExistingIndex"));
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
