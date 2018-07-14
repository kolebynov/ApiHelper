using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Services.Api
{
    public class ApiQuery : IApiQuery
    {
        public async Task<IEnumerable<T>> GetItemsFromQueryAsync<T>(IQueryable<T> query, Guid id, GetItemsOptions options)
            where T : IIdentifiable
        {
            if (!Equals(id, Guid.Empty))
            {
                query = query.Where(entity => entity.Id == id);
            }
            else
            {
                if (options != null && options.RowsCount > 0)
                {
                    query = query.Skip((options.Page.GetValueOrDefault() - 1) * options.RowsCount.Value).Take(options.RowsCount.Value);
                }
            }

            return await Task.FromResult((IEnumerable<T>)query.ToArray());
        }
    }
}