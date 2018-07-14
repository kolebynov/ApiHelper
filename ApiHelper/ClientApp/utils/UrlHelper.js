import modelSchemaProvider from "../schemas/ModelSchemaProvider";
import queryString from "query-string";

class UrlHelper {
    primaryColumnName = "primaryColumnValue";
    resourceName = "resourceName";
    returnUrl = "returnUrl"

    getPathForModelSection() {
        return `/section/:${this.resourceName}`;
    }

    getPathForModelPage() {
        return `/page/:${this.resourceName}/:${this.primaryColumnName}`;
    }

    getUrlForModelSection(modelName, query) {
        const resourceName = modelSchemaProvider.getSchemaByName(modelName).resourceName;
        return this._addQueryToUrl(this.getPathForModelSection().replace(new RegExp(`:${this.resourceName}`), resourceName), query);
    }

    getUrlForModelPage(modelName, primaryColumnValue, query) {
        return this._addQueryToUrl(
            this.getPathForModelPage(modelName)
                .replace(new RegExp(`:${this.primaryColumnName}`), primaryColumnValue)
                .replace(new RegExp(`:${this.resourceName}`), modelSchemaProvider.getSchemaByName(modelName).resourceName),
            query);
    }

    getLoginPageUrl(returnUrl = "/") {
        return `${this.getLoginPagePath()}?${this.returnUrl}=${returnUrl}`;
    }

    getLoginPagePath() {
        return `/login`;
    }

    getDownloadFileUrl(fileId) {
        return `/api/files/${fileId}/fileData`;
    }

    _addQueryToUrl(url, query) {
        return query ? `${url}?${queryString.stringify(query)}` : url;
    }
}

export default new UrlHelper();