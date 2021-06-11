using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses
{
    public class ClassWithHashAndRangeKey
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBRangeKey]
        public string Name { get; set; }
    }
}