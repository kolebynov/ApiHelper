//@flow

export default class SortOption {
    column: string;
    direction: ?number;

    constructor(column: string, direction: ?number) {
        this.column = column;
        this.direction = direction;
    }
};