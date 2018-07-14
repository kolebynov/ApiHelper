import React from "react";
import { Switch, Route } from "react-router-dom";
import SectionRenderer from "../sections/SectionRenderer/SectionRenderer.jsx";
import ModelPageRenderer from "../modelPages/ModelPageRenderer/ModelPageRenderer.jsx";
import urlHelper from "../../utils/UrlHelper";

const Body = () => (
    <Switch>
        <Route path={urlHelper.getPathForModelSection()} component={SectionRenderer}/>
        <Route path={urlHelper.getPathForModelPage()} component={ModelPageRenderer}/>
    </Switch>
);

export default Body;