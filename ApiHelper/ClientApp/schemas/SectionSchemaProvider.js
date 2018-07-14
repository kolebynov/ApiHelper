import schemas from "./SectionSchemas";
import BaseSchemaProvider from "./BaseSchemaProvider";

class SectionSchemaProvider extends BaseSchemaProvider {
    getSchemaByModelName(modelName) {
        return this.getSchemaByFilter(s => s.modelName === modelName, `Section schema by model name "${modelName}" not found.`);
    }
}

export default new SectionSchemaProvider(schemas);