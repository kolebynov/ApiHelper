import React from "react";
import propTypes from "prop-types";
import Divider from 'material-ui/Divider';

class Commentary extends React.PureComponent {
    render() {
        return (
            <div>
                <div>{this.props.date}</div>
                <div>{this.props.text}</div>
                <Divider />
            </div>
        );
    }
}

Commentary.propTypes = {
    text: propTypes.string.isRequired,
    date: propTypes.object.isRequired
};

export default Commentary;