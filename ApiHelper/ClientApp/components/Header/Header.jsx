import React from "react";
import AppBar from 'material-ui/AppBar';
import FlatButton from 'material-ui/FlatButton';
import loginService from "../../services/LoginService";

class Header extends React.Component {
    render() {
        return (
            <div>
                <AppBar iconElementRight={<FlatButton label="Выход" onClick={this._onLogoutButtonClick} />}/>
            </div>
        );
    }

    _onLogoutButtonClick = () => {
        loginService.logout();
    };
}

export default Header;