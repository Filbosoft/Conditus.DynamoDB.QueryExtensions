using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBLocalSecondaryRangeKeyPropertyTests
    {
        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyProperty_WithEntityWithLocalSecondaryRangeKey_ShouldReturnTheLocalSecondaryRangeKeyProperty()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();
            var localSecondaryRangeProperty = typeof(ClassWithDynamoDBAttributes).GetProperty(nameof(entity.Index1Key));

            //When
            var resultRangeKeyProperty = entity.GetDynamoDBLocalSecondaryRangeKeyProperty(ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1);

            //Then
            resultRangeKeyProperty.Should()
                .BeSameAs(localSecondaryRangeProperty);
        }

        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyProperty_WithEntityWithoutLocalSecondaryRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBLocalSecondaryRangeKeyProperty("NonExistingIndex"));
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyProperty_WithTypeOfEntityWithLocalSecondaryRangeKey_ShouldReturnTheLocalSecondaryRangeKeyProperty()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);
            var localSecondaryRangeProperty = typeof(ClassWithDynamoDBAttributes).GetProperty(nameof(ClassWithDynamoDBAttributes.Index1Key));

            //When
            var resultRangeKeyProperty = entityType.GetDynamoDBLocalSecondaryRangeKeyProperty(ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1);

            //Then
            resultRangeKeyProperty.Should()
                .BeSameAs(localSecondaryRangeProperty);
        }

        [Fact]
        public void GetDynamoDBLocalSecondaryRangeKeyProperty_WithTypeOfEntityWithoutLocalSecondaryRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entityType = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entityType.Invoking(e => e.GetDynamoDBLocalSecondaryRangeKeyProperty("NonExistingIndex"));
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
