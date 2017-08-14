using EntityFramework.Extensions.Tests.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFramework.Extensions.Tests
{
    [TestClass]
    public class ExpressionHelperTests
    {
        [TestMethod]
        public void GetMemberName_Ok()
        {
            string memberName
                = ExpressionHelper.GetMemberName( (Parent p) => p.FirstVariable );

            Assert.AreEqual( "FirstVariable", memberName );
        }
    }
}
