using HB.AbpFundation.Samples;
using Xunit;

namespace HB.AbpFundation.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<AbpFundationMongoDbTestModule>
{

}
