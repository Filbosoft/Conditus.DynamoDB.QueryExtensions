using System;
using Conditus.DynamoDB.QueryExtensions.Extensions;
using Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses;
using FluentAssertions;
using Xunit;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.ExtensionTests.EntityDynamoDBExtensionTests
{
    public class GetDynamoDBTableNameTests
    {
        [Fact]
        public void GetDynamoDBTableName_WithEntityWithTableName_ShouldReturnEntitysDynamoDBTableName()
        {
            //Given
            var entity = new ClassWithDynamoDBAttributes();

            //When
            var tableName = entity.GetDynamoDBTableName();

            //Then
            tableName.Should().Be(ClassWithDynamoDBAttributes.DYNAMO_DB_TABLE_NAME);
        }

        [Fact]
        public void GetDynamoDBTableName_WithEntityWithoutTableName_ShouldThrowArgumentException()
        {
            //Given
            var entity = new ClassWithoutDynamoDBAttributes();

            //When
            var action = entity.Invoking(e => e.GetDynamoDBTableName());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDynamoDBTableName_WithTypeOfEntityWithTableName_ShouldReturnEntitysDynamoDBTableName()
        {
            //Given
            var entityType = typeof(ClassWithDynamoDBAttributes);

            //When
            var tableName = entityType.GetDynamoDBTableName();

            //Then
            tableName.Should().Be(ClassWithDynamoDBAttributes.DYNAMO_DB_TABLE_NAME);
        }

        [Fact]
        public void GetDynamoDBTableName_WithTypeOfEntityWithoutTableName_ShouldThrowArgumentException()
        {
            //Given
            var entity = typeof(ClassWithoutDynamoDBAttributes);

            //When
            var action = entity.Invoking(e => e.GetDynamoDBTableName());
                
            //Then
            action.Should().Throw<ArgumentException>();
        }
    }
}
