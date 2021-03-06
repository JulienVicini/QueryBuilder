﻿using QueryBuilder.Core.Helpers;
using QueryBuilder.Core.IQueryables;
using QueryBuilder.Core.Statements;
using QueryBuilder.EF6.SqlServer.Factories;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace QueryBuilder.EF6.SqlServer
{
    public static class IQueryableUpdateExtensions
    {
        public static UpdateStatementBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, Expression<Func<T, TValue>> valueExpression)
        {
            return new UpdateStatementBuilder<T>(queryable)
                        .SetValue(memberExpression, valueExpression);
        }

        public static UpdateStatementBuilder<T> SetValue<T, TValue>(this IQueryable<T> queryable, Expression<Func<T, TValue>> memberExpression, TValue value)
        {
            return new UpdateStatementBuilder<T>(queryable)
                        .SetValue(memberExpression, value);
        }

        public static int Update<T>(this UpdateStatementBuilder<T> updateBuilder) where T : class
        {
            IQueryable<T> queryable = updateBuilder.Queryable;

            // Resolve Service
            var serviceFactory = new ServiceFactory<T>(queryable);
            var commandService = serviceFactory.CreateCommandService();

            // Create & Execute Query
            var query = new UpdateStatement<T>(
                updateBuilder.Assignements,
                IQueryableHelper.GetQueryPredicate(queryable)
            );

            return commandService.Update(query);
        }

        public static int Update<T>(this IQueryable<T> queryable, Expression<Func<T, T>> constructorExpression) where T : class
        {
            // Check & Cast parameters
            Check.NotNull(queryable, nameof(queryable));

            MemberInitExpression memberInit 
                = constructorExpression.Body as MemberInitExpression
                    ?? throw new ArgumentException(nameof(constructorExpression), $"The body of the expression must be of type\"{typeof(MemberInitExpression).Name}\".");

            // Create statement
            var statementBuilder = new UpdateStatementBuilder<T>(queryable);

            foreach(MemberAssignment assignement in memberInit.Bindings)
            {
                statementBuilder.AppendMemberAssignment(assignement);
            }

            // Act
            return Update<T>(statementBuilder);
        }
    }
}
