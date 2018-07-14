import React from "react";
import queryString from "query-string";
import PropTypes from "prop-types";

class StateLoader extends React.PureComponent {
    componentWillMount() {
        const history = this.context.router.history;
        const newState = {...queryString.parse(history.location.search), ...history.location.state};
        history.replace(history.location.pathname + history.location.search, newState);  
    }

    render() {
        return null;
    }
}

StateLoader.contextTypes = {
    router: PropTypes.object.isRequired
};

export default StateLoader;