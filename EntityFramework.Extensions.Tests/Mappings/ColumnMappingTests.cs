using EntityFramework.Extensions.Mappings;
using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Core.Metadata.Edm;

namespace EntityFramework.Extensions.Tests.Mappings
{
    [TestClass]
    public class ColumnMappingTests
    {
        private Context.TestDbContext _dbContext;

        [TestInitialize] public void Init() => _dbContext = new Context.TestDbContext();
        [TestCleanup] public void Clean() => _dbContext.Dispose();

        [TestMethod]
        public void FromMetaDataWhenIdentityColumns()
        {
            EdmProperty edmProperty = _dbContext.GetPropertyMetaData((Parent p) => p.Id);

            var mapping = ColumnMapping.FromMetaDdata(edmProperty);

            Assert.AreEqual(true             , mapping.IsIdentity   );
            Assert.AreEqual(nameof(Parent.Id), mapping.PropertyName );
            Assert.AreEqual("P_Id"           , mapping.SqlName      );
            //Assert.AreEqual(SqlDbType.Int    , mapping.SqlType      );
        }

        [TestMethod]
        public void FromMetaDataWhenColumnsNameNotSet()
        {
            EdmProperty edmProperty = _dbContext.GetPropertyMetaData((Children p) => p.ThirdVariable);

            var mapping = ColumnMapping.FromMetaDdata(edmProperty);

            Assert.AreEqual(false                         , mapping.IsIdentity   );
            Assert.AreEqual(nameof(Children.ThirdVariable), mapping.PropertyName );
            Assert.AreEqual(nameof(Children.ThirdVariable), mapping.SqlName      );
            //Assert.AreEqual(SqlDbType.Int    , mapping.SqlType      );
        }
    }
}
