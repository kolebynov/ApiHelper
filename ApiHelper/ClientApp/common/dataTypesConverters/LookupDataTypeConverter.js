import BaseDataTypeConverter from "./BaseDataTypeConverter";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";
import modelUtils from "../../utils/ModelUtils";

class LookupDataTypeConverter extends BaseDataTypeConverter {
    toString(value, column) {
        const referenceSchema = modelSchemaProvider.getSchemaByName(column.referenceSchemaName);
        return modelUtils.getPrimaryValue(value, referenceSchema);
    }

    fromString(str, column, model) {
        const referenceSchema = modelSchemaProvider.getSchemaByName(column.referenceSchemaName);
        return modelUtils.getLookupCollection(model, column.name).find(value => modelUtils.getPrimaryValue(value, referenceSchema) === str);
    }
}

export default LookupDataTypeConverter;