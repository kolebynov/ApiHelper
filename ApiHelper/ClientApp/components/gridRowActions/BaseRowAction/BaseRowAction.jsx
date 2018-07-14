import React from "react";
import PropTypes from "prop-types";
import FlatButton from 'material-ui/FlatButton';

class BaseRowAction extends React.PureComponent {
    render() {
        return <FlatButton label={this.props.label} onClick={this._onActionButtonClick}/>;
    }

    _onActionButtonClick = () => { }
}

BaseRowAction.propTypes = {
    modelName: PropTypes.string.isRequired,
    model: PropTypes.object.isRequired,
    grid: PropTypes.object.isRequired
};

export default BaseRowAction;