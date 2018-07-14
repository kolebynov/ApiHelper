import getHistory from "../common/History";
import urlHelper from "../utils/UrlHelper";
import cookieUtils from "../utils/CookieUtils";

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

    _request(url, method = "GET", data) {
        return fetch(url, this._getRequestConfig(method, data))
            .then(response => {
                if (response.status === statusCodes.UNAUTHORIZED && window.location.pathname !== urlHelper.getLoginPagePath()) {
                    getHistory().push(urlHelper.getLoginPageUrl(window.location.pathname));
                }
                
                return response.json();
            });
    }

    _getRequestConfig(method = "GET", data) {
        let config = Object.assign({}, this._defaultRequestOptions, { method: method });
        if (method !== "GET" && data) {
            config.body = JSON.stringify(data);
        }
        
        return config;
    }
}

export default BaseService;