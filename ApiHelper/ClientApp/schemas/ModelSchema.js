class ModelSchema {
    constructor(config) {
        Object.assign(this, config);
    }

    getColumnByName(name) {
        let column = this.columns.find(column => column.name === name);
        if (column) {
            return column;
        }
        else {
            throw new Error(`Column ${name} not found`);
        }
    }

    getColumns() {
        return this.columns || [];
    }

    getCaption() {
        return this.caption || this.name;
    }
}

export default ModelSchema;