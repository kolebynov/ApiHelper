import React from "react";
import { Link } from "react-router-dom";
import urlHelper from "../../utils/UrlHelper";
import PropTypes from "prop-types";

const ModelSectionLink = ({modelName, children}) => (
    <Link to={urlHelper.getUrlForModelSection(modelName)}>{children}</Link>
);

ModelSectionLink.propTypes = {
    modelName: PropTypes.string.isRequired
};

export default ModelSectionLink;