import React from "react";
import BaseModelSection from "../BaseModelSection/BaseModelSection.jsx";
import DataGrid from "../../DataGrid/DataGrid.jsx";
import queryString from "query-string";

class NoteSection extends BaseModelSection {
    constructor(props, context) {
        super(props, context);

        this.unlistenHistory = context.router.history.listen(this._onHistoryChange);
        this.state = {
            categoryId: (context.router.route.location.state || {}).category
        };
    }

    componentWillUnmount() {
        this.unlistenHistory();
    }

    _getColumnsForGrid() {
        return ["name", "description"];
    }

    _renderGrid() {
        let rootModel = null;
        const categoryId = this.state.categoryId;
        if (categoryId) {
            rootModel = {
                name: "NoteCategory",
                primaryValue: categoryId
            };
        }
        return (<DataGrid modelName={this.props.modelName} rowActions={this._getDataGridRowActions()} columnsForDisplay={this._getColumnsForGrid()} 
            rootModel={rootModel}/>);
    }

    _onHistoryChange = (location, action) => {
        if ((action === "PUSH" || action === "REPLACE") && location.state && location.state.category) {
            this.setState({
                categoryId: location.state.category
            })
        }
    }
}

export default NoteSection;