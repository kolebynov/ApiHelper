using System.Collections.Generic;
using RestApi.Common;

namespace RestApi.ApiResults
{
    public class ApiResult<TData> : ApiResult
    {
        public TData Data { get; set; }
    }

    public class ApiResult
    {
        public bool Success { get; set; }
        public IEnumerable<ApiError> Errors { get; set; }

        public static ApiResult<TData> SuccessResult<TData>(TData data)
        {
            return new ApiResult<TData>
            {
                Success = true,
                Data = data
            };
        }

        public static GetApiResult<TData> SuccesGetResult<TData>(TData data, PaginationData paginationData)
        {
            return new GetApiResult<TData>
            {
                Success = true,
                Data = data,
                Pagination = paginationData
            };
        }

        public static ApiResult ErrorResult(params ApiError[] errors)
        {
            return ErrorResult((IEnumerable<ApiError>)errors);
        }

        public static ApiResult ErrorResult(IEnumerable<ApiError> errors)
        {
            return new ApiResult
            {
                Success = false,
                Errors = errors
            };
        }
    }
}