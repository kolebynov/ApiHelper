//@flow

import BaseService from "./BaseService";
import queryString from "query-string";
import ApiException from "../exceptions/ApiException";
import GetApiResult from "../ApiResults/GetApiResult";
import ApiResult from "../ApiResults/ApiResult";
import GetOptions from "../Common/GetOptions";

class ApiService<T: Object> extends BaseService {
    apiRoute: string;

    constructor(resourceName: string) {
        super();
        this.apiRoute = `/api/${resourceName}`;
    }

    getItems(id?: string = "", options: ?GetOptions): Promise<GetApiResult<T>> {
        return this._request(`${this.apiRoute}/${id}${options ? "?" + queryString.stringify(options) : ""}`);
    }

    addItem(item: T): Promise<ApiResult<T>> {
        return this._request(this.apiRoute, "POST", item);
    }

    updateItem(id: string, item: T): Promise<ApiResult<T>> {
        return this._request(`${this.apiRoute}/${id}`, "PUT", item);
    }

    deleteItem(id: string): Promise<ApiResult<Object>> {
        return this._request(`${this.apiRoute}/${id}`, "DELETE");
    }

    _request(url: string, method: string = "GET", data: ?Object): Promise<Object> {
        return super._request(url, method, data)
            .then(response => {
                response = (response: ApiResult<Object>);
                if (!response.success) {
                    throw new ApiException(response.errors ?? []);
                }

                return response;
            });
    }
}

export default ApiService;