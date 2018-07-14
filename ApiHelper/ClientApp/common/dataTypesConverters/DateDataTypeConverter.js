import BaseDataTypeConverter from "./BaseDataTypeConverter";

class DateDataTypeConverter extends BaseDataTypeConverter {
    toString(value) {
        return value.toString();
    }

    fromString(str) {
        return new Date(Date.parse(str));
    }
}

export default DateDataTypeConverter;