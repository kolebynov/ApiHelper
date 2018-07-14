import React from "react";
import PropTypes from "prop-types";
import TextField from "material-ui/TextField";
import SelectField from "material-ui/SelectField";
import MenuItem from "material-ui/MenuItem";
import modelUtils from "../../utils/ModelUtils";
import dataTypeConverterProvider from "../../common/DataTypeConverterProvider";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";
import dataTypes from "../../common/DataTypes";

class ModelValueEdit extends React.PureComponent {
    render() {
        let { columnName, model, schema, onChange } = this.props;
        const column = schema.getColumnByName(columnName);
        const value = model[columnName];
        let editComponent = null;
        switch (column.type) {
            case dataTypes.LOOKUP:
                editComponent = this._renderSelectField(value, column, model, onChange);
                break;
            default:
                editComponent = <TextField id={columnName} value={value || ""} onChange={(e, newValue) => 
                    this._onEditComponentChange(newValue, column, model, onChange)} 
                    floatingLabelText={column.getCaption()} />;
        }

        return <div>{editComponent}</div>
    }

    _renderSelectField(value, column, model, onChangeHandler) {
        const referenceSchema = modelSchemaProvider.getSchemaByName(column.referenceSchemaName);
        return (
            <SelectField floatingLabelText={column.getCaption()} value={modelUtils.getPrimaryValue(value, referenceSchema)} 
                onChange={(e, index, newValue) => this._onEditComponentChange(newValue, column, model, onChangeHandler)}>
                {modelUtils.getLookupCollection(model, column.name).map(lookupValue => {
                    const primaryValue = modelUtils.getPrimaryValue(lookupValue, referenceSchema);
                    return (
                        <MenuItem key={primaryValue} value={primaryValue} primaryText={modelUtils.getDisplayValue(lookupValue, referenceSchema)} />
                    );
                })}
            </SelectField>
        );
    }

    _onEditComponentChange(rawValue, column, model, onChangeHandler) {
        const newValue = dataTypeConverterProvider.getConverter(column.type)
            .fromString(rawValue, column, model);
        if (onChangeHandler) {
            onChangeHandler(newValue, column);
        }
    }
}

ModelValueEdit.propTypes = {
    columnName: PropTypes.string.isRequired,
    schema: PropTypes.object.isRequired,
    model: PropTypes.object.isRequired,
    onChange: PropTypes.func
};

export default ModelValueEdit;