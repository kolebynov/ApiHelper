import React from "react";
import ModelPageLink from "../ModelPageLink/ModelPageLink.jsx";
import PropTypes from "prop-types";
import dataTypes from "../../common/DataTypes";
import modelPageSchemaProvider from "../../schemas/ModelPageSchemaProvider";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";
import modelUtils from "../../utils/ModelUtils";

const ModelValueView = ({columnName, schema, model}) => {
    const column = schema.getColumnByName(columnName);
    const value = model[columnName];
    if (column.name === schema.displayColumnName && modelPageSchemaProvider.findSchemaByModelName(schema.name)) {
        return <ModelPageLink modelName={schema.name} primaryValue={modelUtils.getPrimaryValue(model, schema)}>
            {value}</ModelPageLink>
    }
    if (column.type === dataTypes.LOOKUP) {
        let referenceSchema = modelSchemaProvider.getSchemaByName(column.referenceSchemaName);
        if (modelPageSchemaProvider.findSchemaByModelName(column.referenceSchemaName)) {
            return <ModelPageLink modelName={column.referenceSchemaName} 
                primaryValue={modelUtils.getPrimaryValue(value, referenceSchema)}>
                {modelUtils.getDisplayValue(value, referenceSchema)}</ModelPageLink>
        }
        return <span>{modelUtils.getDisplayValue(value, referenceSchema)}</span>;
    }

    return <span>{value}</span>;
};

ModelValueView.propTypes = {
    columnName: PropTypes.string.isRequired,
    schema: PropTypes.object.isRequired,
    model: PropTypes.object.isRequired
};

export default ModelValueView;