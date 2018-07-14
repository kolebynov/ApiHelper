class BaseProvider {
    constructor(items) {
        this._items = items;
    }

    getItems() {
        return this._items;
    }

    getItemByFilter(filter, notFoundMessage) {
        let item = this.getItems().find(filter);
        if (item) {
            return item;
        }
        else {
            throw new Error(notFoundMessage);
        }
    }
}

export default BaseProvider;