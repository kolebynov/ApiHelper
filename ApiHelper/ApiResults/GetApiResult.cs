using Newtonsoft.Json;
using RestApi.Common;

namespace RestApi.ApiResults
{
    public class GetApiResult<TData> : ApiResult<TData>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PaginationData Pagination { get; set; }
    }
}
