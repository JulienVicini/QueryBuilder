using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFramework.Extensions.Tests
{
    [TestClass]
    public class DbContextExtensionsTests
    {
        private TestDbContext _dbContext;

        [TestInitialize]
        public void Init()
        {
            _dbContext = new TestDbContext();
        }

        [TestCleanup]
        public void Clean()
        {
            _dbContext.Dispose();
        }

        [TestMethod]
        public void TestMetaDataWorkspace()
        {
            var metaDatas = _dbContext.GetEntityMetadata<Parent>();
        }

        [TestMethod]
        public void GetPropertyMetaData()
        {
            var metaData = _dbContext.GetPropertyMetaData((Parent p) => p.SecondVariable);
        }

    }
}
