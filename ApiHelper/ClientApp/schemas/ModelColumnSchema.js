class ModelColumnSchema {
    constructor(config) {
        Object.assign(this, config);
    }

    getCaption() {
        return this.caption || this.name;
    }
}

export default ModelColumnSchema;