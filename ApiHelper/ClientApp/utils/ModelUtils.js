import DataTypes from "../common/DataTypes";
import dataTypeConverterProvider from "../common/DataTypeConverterProvider";

class ModelUtils {
    getLookupCollection(model, lookupColumnName) {
        return model[this.getNameForLookupCollection(lookupColumnName)] || [];
    }

    setLookupCollection(model, lookupColumnName, collection) {
        return { ...model, [this.getNameForLookupCollection(lookupColumnName)]: collection };
    }

    getNameForLookupCollection(lookupColumnName) {
        return lookupColumnName + "Collection";
    }

    getPrimaryValue(model, schema) {
        return (model || {})[schema.primaryColumnName];
    }

    getDisplayValue(model, schema) {
        return (model || {})[schema.displayColumnName];
    }

    getModelForUpdate(model, schema) {
        return schema.getColumns().reduce((result, column) => {
            let updateColumnName = column.name;
            if (column.type === DataTypes.LOOKUP) {
                updateColumnName = column.keyColumnName;
            }

            result[updateColumnName] = model[updateColumnName];
            
            return result;
        }, {});
    }
}

export default new ModelUtils();