using HB.AbpFundation.MongoDB;
using HB.AbpFundation.Samples;
using Xunit;

namespace HB.AbpFundation.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<AbpFundationMongoDbTestModule>
{

}
