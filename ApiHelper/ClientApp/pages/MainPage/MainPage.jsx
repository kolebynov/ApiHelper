import Header from "../../components/Header/Header.jsx";
import Body from "../../components/Body/Body.jsx";
import React from "react";
import LeftPanel from "../../components/LeftPanel/LeftPanel.jsx";
import SectionSchemaProvider from "../../schemas/SectionSchemaProvider";
import "./MainPage.css";

const MainPage = () => (
    <div id="MainPage">
        <div id="LeftPanelWrapper">
            <LeftPanel />
        </div>
        <div id="BodyWrapper">
            <Body />
        </div>
    </div>
)

export default MainPage;