using System.Data.Common;

namespace EntityFramework.Extensions.Core.Database
{
    public interface IDatabaseContext
    {

        DbTransaction BeginTransaction();

        DbConnection GetConnection();
    }
}
