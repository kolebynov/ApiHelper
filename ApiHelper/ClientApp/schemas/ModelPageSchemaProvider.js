import schemas from "./ModelPageSchemas";
import BaseSchemaProvider from "./BaseSchemaProvider";

class ModelPageSchemaProvider extends BaseSchemaProvider {
    getSchemaByModelName(modelName) {
        return this.getSchemaByFilter(schema => schema.modelName === modelName, 
            `Schema with model name "${modelName}" not found`);
    }

    findSchemaByModelName(modelName) {
        return this.getSchemas().find(schema => schema.modelName === modelName);
    }
}

export default new ModelPageSchemaProvider(schemas);