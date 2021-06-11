using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses
{
    public class ClassWithHashKey
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}