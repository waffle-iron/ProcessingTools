﻿namespace ProcessingTools.Extensions.Linq
{
    using System;
    using System.Linq;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Extensions.Linq.Expressions;

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByName<T>(this IQueryable<T> query, string propertyName, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return query.OrderBy(keySelector: propertyName.ToExpressionFromPropertyName<T, object>());

                case SortOrder.Descending:
                    return query.OrderByDescending(keySelector: propertyName.ToExpressionFromPropertyName<T, object>());

                default:
                    return query;
            }
        }
    }
}
