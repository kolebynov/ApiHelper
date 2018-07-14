using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestApi.ApiResults;
using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Services.Api
{
    public class ApiHelper : IApiHelper
    {
        private readonly IApiQuery _apiQuery;

        public ApiHelper(IApiQuery apiQuery)
        {
            _apiQuery = apiQuery;
        }

        public ApiResult GetErrorResultFromModelState(ModelStateDictionary modelState)
        {
            IEnumerable<ApiError> errors = modelState.Values
                .SelectMany(s => s.Errors)
                .Select(e => new ApiError { Message = e.ErrorMessage });
            return ApiResult.ErrorResult(errors);
        }

        public ApiResult GetErrorResultFromException(Exception exception)
        {
            return ApiResult.ErrorResult(new[] {GetApiErrorFromException(exception)});
        }

        public async Task<GetApiResult<IEnumerable<T>>> CreateApiResultFromQueryAsync<T>(IQueryable<T> query, Guid id, 
            GetItemsOptions options) where T : IIdentifiable
        {
            int rowsTotal = 1;
            if (Equals(id, Guid.Empty))
            {
                rowsTotal = query.Count();
            }

            return ApiResult.SuccesGetResult(await _apiQuery.GetItemsFromQueryAsync(query, id, options), new PaginationData
            {
                CurrentPage = options?.Page ?? 1,
                ItemsPerPage = options?.RowsCount ?? rowsTotal,
                TotalItems = rowsTotal
            });
        }

        private ApiError GetApiErrorFromException(Exception exception)
        {
            return new ApiError
            {
                Message = exception.Message
            };
        }
    }
}