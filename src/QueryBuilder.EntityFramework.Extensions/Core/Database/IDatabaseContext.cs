using System.Data.Common;

namespace QueryBuilder.EntityFramework.Extensions.Core.Database
{
    public interface IDatabaseContext
    {

        DbTransaction BeginTransaction();

        DbConnection GetConnection();
    }
}
