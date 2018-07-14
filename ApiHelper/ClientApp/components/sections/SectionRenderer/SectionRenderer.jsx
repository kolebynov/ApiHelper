import BaseModelSection from "../BaseModelSection/BaseModelSection.jsx";
import React from "react";
import UrlHelper from "../../../utils/UrlHelper";
import sectionSchemaProvider from "../../../schemas/SectionSchemaProvider";
import modelSchemaProvider from "../../../schemas/ModelSchemaProvider";

class SectionRenderer extends React.PureComponent {
    render() {
        const resourceName = this.props.match.params[UrlHelper.resourceName];
        const modelName = modelSchemaProvider.getSchemaByResourceName(resourceName).name;
        const SectionComponent = sectionSchemaProvider.getSchemaByModelName(modelName).component
            || BaseModelSection;
        return (<SectionComponent modelName={modelName}>{this.props.children}</SectionComponent>);
    }
}

export default SectionRenderer;