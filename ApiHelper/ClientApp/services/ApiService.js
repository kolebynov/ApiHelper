import BaseService from "./BaseService";
import queryString from "query-string";
import ApiException from "../exceptions/ApiException";

class ApiService extends BaseService {
    constructor(resourceName) {
        super();
        this.apiRoute = `/api/${resourceName}`;
    }

    getItems(id, options, linkedResouceName) {
        id = id || "";
        return this._request(`${this.apiRoute}/${id}${linkedResouceName ? `/${linkedResouceName}` : ""}${options ? "?" + queryString.stringify(options) : ""}`);
    }

    addItem(item) {
        return this._request(this.apiRoute, "POST", item);
    }

    updateItem(id, item) {
        return this._request(`${this.apiRoute}/${id}`, "PUT", item);
    }

    deleteItem(id) {
        return this._request(`${this.apiRoute}/${id}`, "DELETE");
    }

    _request(url, method = "GET", data) {
        return super._request(url, method, data)
            .then(response => {
                if (!response.success) {
                    throw new ApiException(response.errors);
                }

                return response;
            });
    }
}

export default ApiService;