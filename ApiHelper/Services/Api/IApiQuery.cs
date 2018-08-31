using RestApi.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Services.Api
{
    public interface IApiQuery
    {
        Task<IEnumerable<T>> GetItemsFromQueryAsync<T>(IQueryable<T> query, GetOptions options)
            where T : IIdentifiable;
    }
}