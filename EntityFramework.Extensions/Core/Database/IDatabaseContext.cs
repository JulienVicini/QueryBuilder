using System.Data.Common;

namespace EntityFramework.Extensions.Core.Database
{
    public interface IDatabaseContext
    {
        DbConnection GetConnection();
    }
}
