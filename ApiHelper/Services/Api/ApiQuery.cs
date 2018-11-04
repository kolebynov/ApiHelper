using RestApi.Common;
using RestApi.Extensions;
using RestApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestApi.Services.Api
{
    public class ApiQuery : IApiQuery
    {
        private readonly IExpressionBuilder _expressionBuilder;

        public ApiQuery(IExpressionBuilder expressionBuilder)
        {
            _expressionBuilder = expressionBuilder ?? throw new ArgumentNullException(nameof(expressionBuilder));
        }

        public virtual async Task<IEnumerable<T>> GetItemsFromQueryAsync<T>(IQueryable<T> query, GetOptions options)
            where T : IIdentifiable
        {
            query.CheckArgumentNull(nameof(query));

            if (options != null)
            {
                query = AddOrderToQuery(AddFiltersToQuery(query, options.Filters), options.Sort);

                if (options.RowsCount > 0)
                {         
                    query = AddPaginationToQuery(query, options.Page, options.RowsCount.Value);
                }
            }

            return await Task.FromResult((IEnumerable<T>)query.ToArray());
        }

        protected virtual IQueryable<T> AddFiltersToQuery<T>(IQueryable<T> query, IEnumerable<Filter> filters)
        {
            if (filters == null)
            {
                return query;
            }

            foreach (Filter filter in filters)
            {
                query = query.Where(_expressionBuilder.GetFilterExpression<T>(filter));
            }

            return query;
        }

        protected virtual IQueryable<T> AddOrderToQuery<T>(IQueryable<T> query, IEnumerable<SortOption> order)
        {
            if (order == null)
            {
                return query;
            }

            foreach (SortOption orderOption in order)
            {
                Func<Expression<Func<T, object>>, IQueryable<T>> orderFunc = null;

                switch (orderOption.Direction)
                {
                    case OrderDirection.Asc:
                        orderFunc = query.OrderBy;
                        break;
                    case OrderDirection.Desc:
                        orderFunc = query.OrderByDescending;
                        break;
                    default:
                        throw new ArgumentException($"Invalid order direction type {orderOption.Direction}", nameof(order));
                }

                query = orderFunc(_expressionBuilder.GetPropertyExpression<T>(orderOption.Column));
            }

            return query;
        }

        protected virtual IQueryable<T> AddPaginationToQuery<T>(IQueryable<T> query, int page, int rowsCount) =>
            query.Skip((page - 1) * rowsCount).Take(rowsCount);
    }
}