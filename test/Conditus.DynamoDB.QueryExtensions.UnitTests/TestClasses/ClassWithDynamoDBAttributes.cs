using Amazon.DynamoDBv2.DataModel;

namespace Conditus.DynamoDB.QueryExtensions.UnitTests.TestClasses
{
    [DynamoDBTable(ClassWithDynamoDBAttributes.DYNAMO_DB_TABLE_NAME)]
    public class ClassWithDynamoDBAttributes
    {
        public const string DYNAMO_DB_TABLE_NAME = "TestEntities";
        
        [DynamoDBHashKey]
        public string HashKey { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBGlobalSecondaryIndexRangeKey(ClassWithDynamoDBAttributesGlobalSecondaryIndexes.Index1)]
        public string RangeKey { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey(ClassWithDynamoDBAttributesGlobalSecondaryIndexes.Index1)]
        [DynamoDBLocalSecondaryIndexRangeKey(ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index1)]
        public string Index1Key { get; set; }

        [DynamoDBLocalSecondaryIndexRangeKey(ClassWithDynamoDBAttributesLocalSecondaryIndexes.Index2)]
        public string Index2Key { get; set; }
    }

    public static class ClassWithDynamoDBAttributesLocalSecondaryIndexes
    {
        public const string Index1 = "Idx1";
        public const string Index2 = "Idx2";
    }

    public static class ClassWithDynamoDBAttributesGlobalSecondaryIndexes
    {
        public const string Index1 = "Idx1";
    }
}