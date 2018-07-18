//@flow
import SortOption from "./SortOption";

export default class GetOptions {
    page: number;
    rowsCount: ?number;
    sort: ?SortOption[];

    constructor(page: number, rowsCount: ?number, sort: ?SortOption[]) {
        this.page = page;
        this.rowsCount = rowsCount;
        this.sort = sort;
    }
}