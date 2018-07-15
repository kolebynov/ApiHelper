using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestApi.ApiResults;
using RestApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Services.Api
{
    public interface IApiHelper
    {
        ApiResult GetErrorResultFromModelState(ModelStateDictionary modelState);
        ApiResult GetErrorResultFromException(Exception exception);
        Task<GetApiResult<IEnumerable<T>>> CreateApiResultFromQueryAsync<T>(IQueryable<T> query, Guid id,
            GetOptions options) where T : IIdentifiable;
    }
}