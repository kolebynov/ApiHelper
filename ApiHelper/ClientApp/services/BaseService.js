//@flow

const statusCodes = {
    UNAUTHORIZED: 401
};

class BaseService {
    _defaultRequestOptions = {
        credentials: "include",
        headers: {
            "Content-Type": "application/json"
        }
    }

    _request(url: string, method: string = "GET", data: ?Object): Promise<Object> {
        return fetch(url, this._getRequestConfig(method, data))
            .then(response => {
                return response.json();
            });
    }

    _getRequestConfig(method: string = "GET", data: ?Object): Object {
        let config = Object.assign({}, this._defaultRequestOptions, { method: method });
        if (method !== "GET" && data) {
            config.body = JSON.stringify(data);
        }
        
        return config;
    }
}

export default BaseService;