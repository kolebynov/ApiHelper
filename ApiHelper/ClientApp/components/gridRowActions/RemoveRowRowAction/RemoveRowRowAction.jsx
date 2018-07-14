import React from "react";
import BaseRowAction from "../BaseRowAction/BaseRowAction.jsx";
import modelUtils from "../../../utils/ModelUtils";
import modelSchemaProvider from "../../../schemas/ModelSchemaProvider";

class RemoveRowRowAction extends BaseRowAction {
    _onActionButtonClick = () => {
        const primaryValue = modelUtils.getPrimaryValue(this.props.model, 
            modelSchemaProvider.getSchemaByName(this.props.modelName));
        this.props.grid.removeRow(primaryValue);
    }
}

export default RemoveRowRowAction;