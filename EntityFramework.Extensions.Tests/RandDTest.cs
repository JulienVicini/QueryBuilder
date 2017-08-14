using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFramework.Extensions.Tests
{
    [TestClass]
    public class RandDTest
    {
        [TestMethod]
        public void Dummy()
        {
            //using(var context = new Context.TestDbContext())
            //{
            //    IQueryable<Parent> unflitered = context.Parents;

            //    IQueryable<Parent> fileted = context.Parents.Where(d => d.Id == 3);

            //    var type = unflitered.Provider.GetType();

            //    MethodCallExpression expr1 = unflitered.Expression as MethodCallExpression;
            //    MethodCallExpression expr2 = fileted.Expression as MethodCallExpression;

            //    //MethodCallExpression objectContextCall = expr1.Arguments.First() as MethodCallExpression;
            //    ObjectQuery objectQuery = (ObjectQuery)((ConstantExpression )expr1.Object).Value;

            //    // Execute Command
            //    objectQuery.Context.ExecuteStoreCommand("DA COMMAND");
            //}
        }
    }
}
