import React from "react";
import "./LoginPage.css";
import ModelSchema from "../../schemas/ModelSchema";
import ModelColumnSchema from "../../schemas/ModelColumnSchema";
import dataTypes from "../../common/DataTypes";
import BaseModelPage from "../../components/modelPages/BaseModelPage/BaseModelPage.jsx";
import loginService from "../../services/LoginService";
import FlatButton from 'material-ui/FlatButton';
import urlHelper from "../../utils/UrlHelper";
import queryString from "query-string";

class LoginPage extends BaseModelPage {
    render() {
        return (
            <div id="loginPage">
                <div className="login-form">
                    <span>Авторизация</span>
                    {this.renderEditComponent("userName")}
                    {this.renderEditComponent("password")}
                    <FlatButton label="Вход" onClick={this._onLoginButtonClick} />
                </div>
            </div>
        )
    }

    _onLoginButtonClick = () => {
        loginService.login(this.state.model)
            .then(response => {
                if (response.success) {      
                    const returnUrl = queryString.parse(this.props.location.search)[urlHelper.returnUrl] || "/";
                    this.context.router.history.push(returnUrl);
                }
            });
    };
}

LoginPage.defaultProps = {
    modelSchema: new ModelSchema({
        name: "LoginData",
        primaryColumnName: "userName",
        displayColumnName: "userName",
        columns: [
            new ModelColumnSchema({
                name: "userName",
                type: dataTypes.TEXT,
                caption: "Логин"
            }),
            new ModelColumnSchema({
                name: "password",
                type: dataTypes.TEXT,
                caption: "Пароль"
            })
        ]
    })
};

export default LoginPage;