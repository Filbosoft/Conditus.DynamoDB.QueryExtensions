using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBHashKeyNameTests
    {
        [Fact]
        public void GetDynamoDBHashKeyName_WithEntityWithHashKey_ShouldReturnTheHashKeyName()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();

            //When
            var resultHashKeyName = entity.GetDynamoDBHashKeyName();

            //Then
            resultHashKeyName.Should().Be(nameof(entity.HashKey));
        }

        [Fact]
        public void GetDynamoDBHashKeyName_WithEntityWithoutHashKey_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBHashKeyName());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBHashKeyName_WithTypeOfEntityWithHashKey_ShouldReturnTheHashKeyName()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);

            //When
            var resultHashKeyName = entityType.GetDynamoDBHashKeyName();

            //Then
            resultHashKeyName.Should().Be(nameof(ClassWithDynamoDBAttributes.HashKey));
        }

        [Fact]
        public void GetDynamoDBHashKeyName_WithTypeOfEntityWithoutHashKey_ShouldThrowArgumentException()
        {
            //Given
            var entityType = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entityType.Invoking(e => e.GetDynamoDBHashKeyName());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
