import React from "react";
import { List, ListItem, makeSelectable } from "material-ui/List";
import UrlHelper from "../../utils/UrlHelper";
import PropTypes from "prop-types";
import ApiService from "../../services/ApiService";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";

const SelectableList = makeSelectable(List);

class LeftPanel extends React.PureComponent {
    constructor(props, context) {
        super(props);

        this.state = {
            selectedSection: null,
            categories: []
        };
    }

    componentWillMount() {
        this._loadCategories();
    }

    render() {
        return (
            <div id="LeftPanel">
                <SelectableList value={this.state.selectedSection} onChange={this._onSelectChange}>
                    {this.state.categories.map(category => 
                        (<ListItem key={category.id} primaryText={category.name} data-primary-value={category.id} 
                            onClick={this._onListItemClick} value={category.id}></ListItem>)
                    )}
                </SelectableList>
            </div>
        );
    }

    _goToCategory(categoryId) {
        const url = UrlHelper.getUrlForModelSection("Note", {category: categoryId});
        this.context.router.history.push(url, {...this.context.router.route.location.state, category: categoryId});
    }

    _onListItemClick = (e) => {
        this._goToCategory(e.currentTarget.dataset.primaryValue);
    }
    
    _onSelectChange = (e, modelName) => {
        this.setState({
            selectedSection: modelName
        });
    }

    _loadCategories() {
        const categorySchema = modelSchemaProvider.getSchemaByName("NoteCategory");
        new ApiService(categorySchema.resourceName).getItems()
            .then(response => this.setState({
                categories: response.data
            }));
    }
}

LeftPanel.contextTypes = {
    router: PropTypes.object.isRequired
};

export default LeftPanel;