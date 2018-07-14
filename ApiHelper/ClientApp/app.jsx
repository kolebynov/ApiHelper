import React from "react";
import MainPage from "./pages/MainPage/MainPage.jsx";
import LoginPage from "./pages/LoginPage/LoginPage.jsx";
import { Switch, Route } from "react-router-dom";
import MuiThemeProvider from "material-ui/styles/MuiThemeProvider";
import urlHelper from "./utils/UrlHelper";
import { GlobalHistory } from "./common/History";
import StateLoader from "./components/StateLoader.jsx";
import "./app.css";

const App = () => (
    <MuiThemeProvider>
        <div>
            <GlobalHistory />
            <Switch>
                <Route path={urlHelper.getLoginPagePath()} component={LoginPage} />
                <Route path="/" component={MainPage} />
            </Switch>
            <StateLoader />
        </div>
    </MuiThemeProvider>
)

export default App;