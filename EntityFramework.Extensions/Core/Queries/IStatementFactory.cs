//using EntityFramework.Extensions.Core.Queries.Generator;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

//namespace EntityFramework.Extensions.Core.Queries
//{
//    public interface IQueryFactory<TEntity, TQueryParam>
//        where TEntity : class
//    {
//        IStatement<TQueryParam> CreateDeleteQuery( Expression<Func<TEntity, bool>> predicate );

//        IStatement<TQueryParam> CreateUpdateQuery( Expression<Func<TEntity, bool>> predicate, IEnumerable<Expression<Action<TEntity>>> assignValueExpressions );
//    }
//}
