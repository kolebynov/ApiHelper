import React from "react";
import PropTypes from "prop-types";
import urlHelper from "../../../utils/UrlHelper";
import BaseRowAction from "../BaseRowAction/BaseRowAction.jsx";
import modelUtils from "../../../utils/ModelUtils";
import modelSchemaProvider from "../../../schemas/ModelSchemaProvider";

class OpenPageRowAcion extends BaseRowAction {
    _onActionButtonClick = () => {
        const primaryValue = modelUtils.getPrimaryValue(this.props.model, 
            modelSchemaProvider.getSchemaByName(this.props.modelName));
        const url = urlHelper.getUrlForModelPage(this.props.modelName, primaryValue);
        this.context.router.history.push(url);
    }
}

OpenPageRowAcion.contextTypes = {
    router: PropTypes.object.isRequired
};

export default OpenPageRowAcion;