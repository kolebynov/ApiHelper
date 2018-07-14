import BaseService from "./BaseService";

class LoginService extends BaseService {
    _loginUrl = "/api/account/login";
    _logoutUrl = "/api/account/logout";

    login(loginData) {
        return this._request(this._loginUrl, "POST", loginData);
    }

    logout() {
        return this._request(this._logoutUrl, "POST");
    }
}

export default new LoginService();