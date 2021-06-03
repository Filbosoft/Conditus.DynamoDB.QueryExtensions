using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBHashKeyPropertyTests
    {
        [Fact]
        public void GetDynamoDBHashKeyProperty_WithEntityWithHashKey_ShouldReturnTheHashKeyProperty()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();
            var hashProperty = typeof(ClassWithDynamoDBAttributes).GetProperty(nameof(entity.HashKey));

            //When
            var resultHashKeyProperty = entity.GetDynamoDBHashKeyProperty();

            //Then
            resultHashKeyProperty.Should()
                .BeSameAs(hashProperty);
        }

        [Fact]
        public void GetDynamoDBHashKeyProperty_WithEntityWithoutHashKey_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBHashKeyProperty());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBHashKeyProperty_WithTypeOfEntityWithHashKey_ShouldReturnTheHashKeyProperty()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);
            var hashProperty = typeof(ClassWithDynamoDBAttributes).GetProperty(nameof(ClassWithDynamoDBAttributes.HashKey));

            //When
            var resultHashKeyProperty = entityType.GetDynamoDBHashKeyProperty();

            //Then
            resultHashKeyProperty.Should()
                .BeSameAs(hashProperty);
        }

        [Fact]
        public void GetDynamoDBHashKeyProperty_WithTypeOfEntityWithoutHashKey_ShouldThrowArgumentException()
        {
            //Given
            var entityType = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entityType.Invoking(e => e.GetDynamoDBHashKeyProperty());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
