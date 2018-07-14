import React from "react";
import modelPageSchemaProvider from "../../../schemas/ModelPageSchemaProvider";
import modelSchemaProvider from "../../../schemas/ModelSchemaProvider";
import BaseModelPage from "../BaseModelPage/BaseModelPage.jsx";
import urlHelper from "../../../utils/UrlHelper";

class ModelPageRenderer extends React.PureComponent {
    render() {
        const resourceName = this.props.match.params[urlHelper.resourceName];
        const primaryColumnValue = this.props.match.params[urlHelper.primaryColumnName];
        const modelSchema = modelSchemaProvider.getSchemaByResourceName(resourceName);
        const ModelPageComponent = modelPageSchemaProvider.getSchemaByModelName(modelSchema.name).component
            || BaseModelPage;
        return <ModelPageComponent modelSchema={modelSchema} primaryColumnValue={primaryColumnValue} 
            initialModel={this.props.location.state}>{this.props.children}</ModelPageComponent>
    }
}

export default ModelPageRenderer;