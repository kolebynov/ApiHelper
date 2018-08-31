using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using RestApi.ApiResults;
using RestApi.Common;
using RestApi.Configuration;
using RestApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Services.Api
{
    public class ApiHelper : IApiHelper
    {
        private readonly IApiQuery _apiQuery;
        private readonly bool _showFullErrorInfo;

        public ApiHelper(IApiQuery apiQuery, IOptions<RestApiOptions> options)
        {
            _apiQuery = apiQuery ?? throw new ArgumentNullException(nameof(apiQuery));
            _showFullErrorInfo = options.Value.ApiException.ShowFullErrorInfo;
        }

        public virtual ApiResult GetErrorResultFromModelState(ModelStateDictionary modelState)
        {
            modelState.CheckArgumentNull(nameof(modelState));

            IEnumerable<ApiError> errors = modelState.Values
                .SelectMany(s => s.Errors)
                .Select(e => new ApiError { Message = e.ErrorMessage });
            return ApiResult.ErrorResult(errors);
        }

        public virtual ApiResult GetErrorResultFromException(Exception exception)
        {
            exception.CheckArgumentNull(nameof(exception));

            return ApiResult.ErrorResult(GetApiErrorFromException(exception));
        }

        public virtual async Task<GetApiResult<IEnumerable<T>>> CreateApiResultFromQueryAsync<T>(IQueryable<T> query,
            GetOptions options) where T : IIdentifiable
        {
            query.CheckArgumentNull(nameof(query));

            int rowsTotal = query.Count();

            return ApiResult.SuccesGetResult(await _apiQuery.GetItemsFromQueryAsync(query, options), new PaginationData
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
                Message = _showFullErrorInfo ? exception.ToString() : exception.Message
            };
        }
    }
}