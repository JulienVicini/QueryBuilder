using System;
using System.Data.Common;
using System.Data.Entity.Core.Objects;

namespace EntityFramework.Extensions.Core.Database
{
    public class ObjectContextDatabaseContextAdapter
        : IDatabaseContext/*, ICommandProcessing*/
    {
        private readonly ObjectContext _objectContext;

        public ObjectContextDatabaseContextAdapter(ObjectContext objectContext)
        {
            _objectContext = objectContext ?? throw new ArgumentNullException(nameof(objectContext));
        }

        public DbConnection GetConnection()
        {
            return _objectContext.Connection;
        }

        //public int Execute(Query query)
        //{
        //    if (query == null) throw new ArgumentNullException(nameof(query));

        //    return _objectContext.ExecuteStoreCommand(
        //        commandText: query.QueryString,
        //        parameters : query.Parameters
        //    );
        //} 
    }
}
