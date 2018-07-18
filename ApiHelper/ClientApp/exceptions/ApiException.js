//@flow
import ApiError from "../ApiResults/ApiError";

class ApiException {
    _apiErrors: ApiError[];
    _errorString: string;

    constructor(apiErrors: ApiError[]) {
        this._apiErrors = apiErrors;
    }

    getErrors(): ApiError[] {
        return this._apiErrors;
    }

    toString() {
        return this._errorString || (this._errorString = this._createErrorString());
    }

    _createErrorString(): string {
        return this.getErrors().join("\n");
    }
}

export default ApiException;