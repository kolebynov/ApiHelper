import React from "react";
import DataGrid from "../DataGrid/DataGrid.jsx";
import PropTypes from "prop-types";
import {Toolbar, ToolbarGroup, ToolbarSeparator, ToolbarTitle} from 'material-ui/Toolbar';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import urlHelper from "../../utils/UrlHelper";
import constants from "../../utils/Constants";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";
import ApiService from "../../services/ApiService";
import modelUtils from "../../utils/ModelUtils";
import ContentAdd from 'material-ui/svg-icons/content/add';
import RemoveRowRowAction from "../gridRowActions/RemoveRowRowAction/RemoveRowRowAction.jsx";

class Detail extends React.PureComponent {
    render() {
        return (
            <div>
                {this._renderToolbar()}
                <DataGrid modelName={this.props.modelName} rootModel={this.props.rootModel} itemsPerPage={10} 
                    rowActions={this._getGridRowActions()}/>
            </div>
        );
    }

    addRecord() {
        this._getModelForNewRecord()
            .then(newModel => 
                this.context.router.history.push(urlHelper.getUrlForModelPage(this.props.modelName, constants.EMPTY_GUID), newModel));
    }

    _renderToolbar() {
        return (
            <Toolbar>
                <ToolbarGroup>
                    <ToolbarTitle text={this.props.caption}/>
                    <FloatingActionButton mini={true} onClick={this._onAddButtonClick}>
                        <ContentAdd />
                    </FloatingActionButton>
                </ToolbarGroup>
                <ToolbarGroup>

                </ToolbarGroup>
            </Toolbar>
        );
    }

    _onAddButtonClick = () => {
        this.addRecord();
    }

    _getModelForNewRecord() {
        const column = modelSchemaProvider.getSchemaByName(this.props.modelName).getColumns()
            .find(column => column.referenceSchemaName === this.props.rootModel.name);
        if (!column) {
            return new Promise((res) => res({}));
        }
        const rootSchema = modelSchemaProvider.getSchemaByName(this.props.rootModel.name);
        return new ApiService(rootSchema.resourceName)
            .getItems(this.props.rootModel.primaryValue)
            .then(response => {
                return {
                    [column.name]: response.data[0],
                    [column.keyColumnName]: modelUtils.getPrimaryValue(response.data[0], rootSchema)
                };
            });
    }

    _getGridRowActions() {
        return [
            {
                component: RemoveRowRowAction,
                props: {
                    label: "Удалить"
                }
            }
        ];
    }
}

Detail.propTypes = {
    modelName: PropTypes.string.isRequired,
    rootModel: PropTypes.object.isRequired,
    caption: PropTypes.string.isRequired
};

Detail.contextTypes = {
    router: PropTypes.object.isRequired
};

export default Detail;