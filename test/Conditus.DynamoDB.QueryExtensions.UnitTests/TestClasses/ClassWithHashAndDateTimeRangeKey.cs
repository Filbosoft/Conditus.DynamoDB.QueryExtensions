using System;
using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses
{
    public class ClassWithHashAndDateTimeRangeKey
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBRangeKey]
        public DateTime CreatedAt { get; set; }
    }
}