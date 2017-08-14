using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EntityFramework.Extensions.Tests
{
    [TestClass]
    public class SqlParameterCollectionTest
    {
        [TestMethod]
        public void AddReturnIndexedParameterName()
        {
            var parameterCollection = new SqlParameterCollection();

            var param1Name = parameterCollection.AddParameter( 1        );//, SqlDbType.Int      ); 
            var param2Name = parameterCollection.AddParameter( "sdsqsd" );//, SqlDbType.NVarChar );
            var param3Name = parameterCollection.AddParameter( true     );//, SqlDbType.Bit      ); 
            var param4Name = parameterCollection.AddParameter( 1m       );//, SqlDbType.Decimal  );

            Assert.AreEqual("@p0", param1Name);
            Assert.AreEqual("@p1", param2Name);
            Assert.AreEqual("@p2", param3Name);
            Assert.AreEqual("@p3", param4Name);
        }

        [TestMethod]
        public void Add()
        {
            // create parameters
            var parameterCollection = new SqlParameterCollection();

            var param1Name = parameterCollection.AddParameter( 1        );//, SqlDbType.Int      ); 
            var param2Name = parameterCollection.AddParameter( "sdsqsd" );//, SqlDbType.NVarChar );
            var param3Name = parameterCollection.AddParameter( true     );//, SqlDbType.Bit      ); 
            var param4Name = parameterCollection.AddParameter( 1m       );//, SqlDbType.Decimal  );

            // assert parameters names
            Assert.AreEqual(4, parameterCollection.Parameters.Count());

            Assert.AreEqual( "@p0", param1Name );
            Assert.AreEqual( "@p1", param2Name );
            Assert.AreEqual( "@p2", param3Name );
            Assert.AreEqual( "@p3", param4Name );

            // Assert If parameters exist
            SqlParameter parameter1 = parameterCollection.Parameters.FirstOrDefault(p => p.ParameterName == param1Name ),
                         parameter2 = parameterCollection.Parameters.FirstOrDefault(p => p.ParameterName == param2Name ),
                         parameter3 = parameterCollection.Parameters.FirstOrDefault(p => p.ParameterName == param3Name ),
                         parameter4 = parameterCollection.Parameters.FirstOrDefault(p => p.ParameterName == param4Name );

            // Assert Parameters exists
            Assert.IsNotNull(parameter1);
            Assert.IsNotNull(parameter2);
            Assert.IsNotNull(parameter3);
            Assert.IsNotNull(parameter4);

            // Assert SqlType
            Assert.AreEqual( parameter1.SqlDbType, SqlDbType.Int      );
            Assert.AreEqual( parameter2.SqlDbType, SqlDbType.NVarChar );
            Assert.AreEqual( parameter3.SqlDbType, SqlDbType.Bit      );
            Assert.AreEqual( parameter4.SqlDbType, SqlDbType.Decimal  );

            // Assert Value
            Assert.AreEqual( parameter1.Value, 1        );
            Assert.AreEqual( parameter2.Value, "sdsqsd" );
            Assert.AreEqual( parameter3.Value, true     );
            Assert.AreEqual( parameter4.Value, 1m       );
        }
    }
}
