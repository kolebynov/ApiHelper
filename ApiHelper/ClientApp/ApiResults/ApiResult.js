//@flow
import ApiError from "./ApiError";

export default class ApiResult<TData> {
    success: boolean;
    errors: ?ApiError[];
    data: ?TData;
};