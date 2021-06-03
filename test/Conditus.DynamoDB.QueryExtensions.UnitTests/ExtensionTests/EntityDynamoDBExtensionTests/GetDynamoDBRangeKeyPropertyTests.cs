using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBRangeKeyPropertyTests
    {
        [Fact]
        public void GetDynamoDBRangeKeyProperty_WithEntityWithRangeKey_ShouldReturnTheRangeKeyProperty()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();
            var rangeProperty = typeof(ClassWithDynamoDBAttributes).GetProperty(nameof(entity.RangeKey));

            //When
            var resultRangeKeyProperty = entity.GetDynamoDBRangeKeyProperty();

            //Then
            resultRangeKeyProperty.Should()
                .BeSameAs(rangeProperty);
        }

        [Fact]
        public void GetDynamoDBRangeKeyProperty_WithEntityWithoutRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBRangeKeyProperty());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBRangeKeyProperty_WithTypeOfEntityWithRangeKey_ShouldReturnTheRangeKeyProperty()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);
            var rangeProperty = typeof(ClassWithDynamoDBAttributes).GetProperty(nameof(ClassWithDynamoDBAttributes.RangeKey));

            //When
            var resultRangeKeyProperty = entityType.GetDynamoDBRangeKeyProperty();

            //Then
            resultRangeKeyProperty.Should()
                .BeSameAs(rangeProperty);
        }

        [Fact]
        public void GetDynamoDBRangeKeyProperty_WithTypeOfEntityWithoutRangeKey_ShouldThrowArgumentException()
        {
            //Given
            var entityType = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entityType.Invoking(e => e.GetDynamoDBRangeKeyProperty());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
