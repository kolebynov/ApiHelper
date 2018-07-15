using RestApi.Common;

namespace RestApi.ApiResults
{
    public class GetApiResult<TData> : ApiResult<TData>
    {
        public PaginationData Pagination { get; set; }
    }
}
