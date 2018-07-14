import React from "react";
import BaseModelPage from "../BaseModelPage/BaseModelPage.jsx";
import ApiService from "../../../services/ApiService";
import modelSchemaProvider from "../../../schemas/ModelSchemaProvider";
import PropTypes from "prop-types";
import modelUtils from "../../../utils/ModelUtils";
import dataTypeConverterProvider from "../../../common/DataTypeConverterProvider";
import DataTypes from "../../../common/DataTypes";
import FlatButton from 'material-ui/FlatButton';
import constants from "../../../utils/Constants";

class BaseModelSchemaPage extends BaseModelPage {
    componentWillMount() {
        super.componentWillMount();
        this._loadModel();
    }

    _renderHeader() {
        return (
            <div>
                <FlatButton label="Сохранить" onClick={this._save}></FlatButton>
                <FlatButton label="Назад" onClick={this._back}></FlatButton>
            </div>
        );
    }

    _loadModel() {
        if (!this._isEmptyGuid(this.props.primaryColumnValue)) {
            new ApiService(this.props.modelSchema.resourceName)
                .getItems(this.props.primaryColumnValue)
                .then(response => this.setState({
                    model: { ...response.data[0], ...this.state.model }
                }));
        }
    }

    _save = () => {
        if (this.state.hasChanges) {
            this._getSavePromise().then(response => this._back());
        }
        else {
            this._back();
        }
    }

    _back = () => {
        this.context.router.history.goBack();
    }

    _isEmptyGuid(guid) {
        return !guid || guid === constants.EMPTY_GUID;
    }

    _getSavePromise() {
        const modelSchema = this.props.modelSchema;
        const model = modelUtils.getModelForUpdate(this.state.model, modelSchema);
        const apiService = new ApiService(modelSchema.resourceName);

        if (!this._isEmptyGuid(this.props.primaryColumnValue)) {
            return apiService.updateItem(modelUtils.getPrimaryValue(model, modelSchema), model);
        }
        else {
            return apiService.addItem(model);
        }
    }
}

BaseModelSchemaPage.propTypes = {
    primaryColumnValue: PropTypes.string
}

BaseModelSchemaPage.contextTypes = {
    router: PropTypes.object.isRequired
};

export default BaseModelSchemaPage;