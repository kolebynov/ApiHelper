//@flow
import ApiResult from "./ApiResult";
import PaginationData from "../Common/PaginationData";

export default class GetApiResult<TData> extends ApiResult<TData> {
    pagination: PaginationData
};