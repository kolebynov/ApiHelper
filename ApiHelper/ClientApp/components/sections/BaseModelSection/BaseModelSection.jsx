import React from "react";
import PropTypes from "prop-types";
import modelSchemaProvider from "../../../schemas/ModelSchemaProvider";
import DataGrid from "../../DataGrid/DataGrid.jsx";
import ApiService from "../../../services/ApiService";
import Pagination from "../../Pagination/Pagination.jsx";
import constants from "../../../utils/Constants";
import FlatButton from 'material-ui/FlatButton';
import urlHelper from "../../../utils/UrlHelper";
import modelPageSchemaProvider from "../../../schemas/ModelPageSchemaProvider";
import OpenPageRowAction from "../../gridRowActions/OpenPageRowAcion/OpenPageRowAcion.jsx";
import RemoveRowRowAction from "../../gridRowActions/RemoveRowRowAction/RemoveRowRowAction.jsx";

class BaseModelSection extends React.PureComponent {
    constructor(props) {
        super(props);
        this.state = {}
    }

    render() {
        return (
            <div>
                {this.renderHeader()}
                {this._renderGrid()}
            </div>
        );
    }

    renderHeader() {
        return (
            <div>
                {this.renderHeaderButtons()}
            </div>
        );
    }

    renderHeaderButtons() {
        return (
            <div>
                <FlatButton label="Добавить" onClick={this._onAddButtonClick}></FlatButton>
            </div>
        );
    }

    _renderGrid() {
        return (<DataGrid modelName={this.props.modelName} rowActions={this._getDataGridRowActions()} columnsForDisplay={this._getColumnsForGrid()}/>);
    }

    openEditPage(primaryValue) {
        const url = urlHelper.getUrlForModelPage(this.props.modelName, primaryValue);
        this.context.router.history.push(url);
    }

    _onAddButtonClick = () => {
        this.openEditPage(constants.EMPTY_GUID);
    }

    _getDataGridRowActions() {
        const actions = [];
        if (modelPageSchemaProvider.findSchemaByModelName(this.props.modelName)) {
            actions.push({
                component: OpenPageRowAction,
                props: {
                    label: "Открыть"
                }
            });
        }
        
        actions.push({
            component: RemoveRowRowAction,
            props: {
                label: "Удалить"
            }
        });

        return actions;
    }

    _getColumnsForGrid() {
        return null;
    }
}

BaseModelSection.propTypes = {
    modelName: PropTypes.string.isRequired
};

BaseModelSection.contextTypes = {
    router: PropTypes.object.isRequired
};

export default BaseModelSection;