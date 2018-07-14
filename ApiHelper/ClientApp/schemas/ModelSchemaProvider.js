import schemas from "./Schemas";
import BaseSchemaProvider from "./BaseSchemaProvider";

class ModelSchemaProvider extends BaseSchemaProvider {
    getSchemaByName(name) {
        return this.getSchemaByFilter(s => s.name === name, `Model schema "${name}" not found.`);
    }

    getSchemaByResourceName(resourceName) {
        return this.getSchemaByFilter(s => s.resourceName === resourceName, `Model schema by resource name "${resourceName}" not found.`);
    }
}

export default new ModelSchemaProvider(schemas);