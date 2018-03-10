using QueryBuilder.Core.Database;
using QueryBuilder.Core.Mappings;
using QueryBuilder.Core.Statements;
using QueryBuilder.SqlServer.Statements;
using System.Linq;

namespace QueryBuilder.EFCore.SqlServer.Factories
{
    public class StatementFacadeFactory<T>
        where T : class
    {

        private readonly MappingAdapterFactory<T> _mappingAdapterFactory;

        private readonly DatabaseAdapterFactory<T> _databaseAdapterFactory;

        public StatementFacadeFactory()
        {
            _mappingAdapterFactory  = new MappingAdapterFactory<T>();
            _databaseAdapterFactory = new DatabaseAdapterFactory<T>();
        }

        public StatementFacade<T> CreateFacade(IQueryable<T> queryable)
        {
            IMappingAdapter<T> mappingAdapter = _mappingAdapterFactory.CreateMappingAdapter(queryable);
            ICommandProcessing commandProcessor = _databaseAdapterFactory.CreateCommandProcessor(queryable);

            return new StatementFacade<T>(
                new SqlQueryTranslator<T>(mappingAdapter),
                commandProcessor
            );
        }
    }
}
