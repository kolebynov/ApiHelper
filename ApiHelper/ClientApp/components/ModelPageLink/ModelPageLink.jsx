import React from "react";
import { Link } from "react-router-dom";
import urlHelper from "../../utils/UrlHelper";
import PropTypes from "prop-types";

const ModelPageLink = ({modelName, primaryValue, children}) => (
    <Link to={urlHelper.getUrlForModelPage(modelName, primaryValue)}>{children}</Link>
);

ModelPageLink.propTypes = {
    modelName: PropTypes.string.isRequired,
    primaryValue: PropTypes.string.isRequired
};

export default ModelPageLink;